using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.DbModels;
using Models.Dto;
using Services.Options;

namespace Services
{
    public class AdvertService : IAdvertService
    {
        private readonly BulletinBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly AdvertOptions _options;
        public AdvertService(IMapper mapper, BulletinBoardDbContext dbContext, IOptions<AdvertOptions> options, IImageService imageService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _options = options.Value;
            _imageService = imageService;
        }
        public async Task<Guid> CreateAdvert(Guid userId, string text, List<IFormFile> images, bool isDraft)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            int numAdverts = await _dbContext.Adverts.CountAsync(a => a.UserId == user.Id);

            int maxAdvertsPerUser = _options.MaxAdvertsPerUser;

            if (numAdverts >= maxAdvertsPerUser)
            {
                throw new Exception($"Максимальное количество объявлений, которые вы можете выложить: {maxAdvertsPerUser}");
            }

            var newAdvert = new Advert
            {
                Id = Guid.NewGuid(),
                User = user,
                Text = text,
                IsDraft = isDraft,
                TimeCreated = DateTime.UtcNow,
                ExpirationDate = DateTime.Now.AddDays(_options.ExpirationDate),
                AdvertImages = new List<AdvertImage>()
            };

            foreach (var image in images)
            {
                var filePath = await _imageService.UploadImage(image);
                var advertImage = new AdvertImage
                {
                    FileName = image.FileName,
                    FileType = image.ContentType,
                    FilePath = filePath,
                    Advert = newAdvert
                };
                newAdvert.AdvertImages.Add(advertImage);
            }

            _dbContext.Adverts.Add(newAdvert);
            await _dbContext.SaveChangesAsync();

            return newAdvert.Id;
        }
        public async Task<AdvertDto> GetAdvertById(Guid advertId, Guid userId)
        {
            var advert = await _dbContext.Adverts
                .Include(a => a.AdvertImages)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == advertId);

            if (advert == null || (advert.IsDraft && advert.User.Id != userId))
            {
                throw new Exception("Объявление не найдено");
            }

            var advertDto = _mapper.Map<AdvertDto>(advert);

            return advertDto;
        }
        public async Task<IList<AdvertDto>> GetAllPublishedAdverts()
        {
            var adverts = await _dbContext.Adverts.Where(a => a.IsDraft == false)
                .Include(a => a.User)
                .Include(a => a.AdvertImages)
                .Include(a => a.AdvertReaction)
                .ToListAsync();

            var advertsDto = _mapper.Map<List<AdvertDto>>(adverts);

            return advertsDto;
        }
        public async Task<List<AdvertDto>> SearchAdverts(string? searchText, AdvertSortOrder sortOrder, DateTime? startDate, DateTime? endDate)
        {

            var adverts = await _dbContext.Adverts
                .Include(a => a.User)
                .Include(a => a.AdvertImages)
                .Include(a => a.AdvertReaction)
                .Where(a => a.Text.Contains(searchText) && !a.IsDraft)
                .ToListAsync();

            var advertsDto = _mapper.Map<List<AdvertDto>>(adverts);

            advertsDto = SortAdverts(advertsDto, sortOrder, startDate, endDate);

            return advertsDto;
        }
        public async Task DeleteAdvert(Guid advertId, Guid userId)
        {
            var advert = await _dbContext.Adverts
            .Include(a => a.User)
            .Include(a => a.AdvertImages)
            .SingleOrDefaultAsync(a => a.Id == advertId);

            var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId);

            if (advert == null || (advert.IsDraft && advert.User.Id != userId))
            {
                throw new Exception("Объявление не найдено");
            }
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }
            if (!user.Admin && advert.User.Id != userId)
            {
                throw new Exception("У вас нет прав удалять данное объявление");
            }
            if (advert.AdvertImages != null)
            {
                foreach (var image in advert.AdvertImages)
                {
                    _dbContext.AdvertImages.Remove(image);
                    _imageService.DeleteImage(image.FilePath);
                }
            }
            _dbContext.Adverts.Remove(advert);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAdvert(Guid advertId, Guid userId, string text, bool isDraft, List<IFormFile> newImages, List<Guid> imagesToDelete)
        {
            var advert = await _dbContext.Adverts.Include(a => a.AdvertImages).FirstOrDefaultAsync(a => a.Id == advertId);

            if (advert == null)
            {
                throw new Exception("Объявление не найдено");
            }
            if (advert.User.Id != userId)
            {
                throw new Exception("У вас нет прав изменять данное объявление");
            }
            if (text != null)
            {
                advert.Text = text;
            }
            if (advert.IsDraft)
            {
                advert.IsDraft = isDraft;
            }

            advert.ExpirationDate = DateTime.Now.AddDays(_options.ExpirationDate);

            if (advert.AdvertImages != null)
            {
                foreach (var imageToDeleteId in imagesToDelete)
                {
                    var advertImage = advert.AdvertImages.FirstOrDefault(ai => ai.Id == imageToDeleteId);
                    if (advertImage != null)
                    {
                        _dbContext.AdvertImages.Remove(advertImage);
                        _imageService.DeleteImage(advertImage.FilePath);
                    }
                }
            }

            foreach (var newImage in newImages)
            {
                var filePath = await _imageService.UploadImage(newImage);
                var advertImage = new AdvertImage
                {
                    FileName = newImage.FileName,
                    FileType = newImage.ContentType,
                    FilePath = filePath,
                    Advert = advert
                };
                advert.AdvertImages.Add(advertImage);
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task ReactToAdvert(Guid userId, Guid advertId, Reaction reaction)
        {
            var existingReaction = await _dbContext.AdvertReactions.FirstOrDefaultAsync(r => r.UserId == userId && r.AdvertId == advertId);
            if (existingReaction != null)
            {
                existingReaction.Reaction = reaction;
            }
            else
            {
                var newReaction = new AdvertReaction
                {
                    UserId = userId,
                    AdvertId = advertId,
                    Reaction = reaction
                };
                _dbContext.AdvertReactions.Add(newReaction);
            }
            await _dbContext.SaveChangesAsync();
        }
        private List<AdvertDto> SortAdverts(List<AdvertDto> adverts, AdvertSortOrder sortOrder, DateTime? startDate, DateTime? endDate)
        {
            adverts = adverts.Where(a =>
            (!startDate.HasValue || a.TimeCreated >= startDate.Value)
            && (!endDate.HasValue || a.TimeCreated <= endDate.Value))
            .ToList();

            switch (sortOrder)
            {
                case AdvertSortOrder.CreationDateAsc:
                    adverts = adverts.OrderBy(a => a.TimeCreated).ToList();
                    break;
                case AdvertSortOrder.CreationDateDesc:
                    adverts = adverts.OrderByDescending(a => a.TimeCreated).ToList();
                    break;
                case AdvertSortOrder.RatingAsc:
                    adverts = adverts.OrderBy(a => a.Reactions.Sum(r => (int)r.Reaction)).ToList();
                    break;
                case AdvertSortOrder.RatingDesc:
                    adverts = adverts.OrderByDescending(a => a.Reactions.Sum(r => (int)r.Reaction)).ToList();
                    break;
                default:
                    break;
            }
            return adverts;
        }
    }
}