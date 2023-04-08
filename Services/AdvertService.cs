using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class AdvertService
    {
        private readonly BulletinBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IImageService _imageService;
        public AdvertService(IMapper mapper, BulletinBoardDbContext dbContext, IConfiguration config, IImageService imageService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _config = config;
            _imageService = imageService;
        }

        public async Task<Guid> CreateAdvert(AdvertDto advertDto, List<IFormFile> images)
        {
            var user = await _dbContext.Users.FindAsync(advertDto.UserId);

            int numAdverts = await _dbContext.Adverts.CountAsync(a => a.User.Id == user.Id);

            int maxAdvertsPerUser = _config.GetValue<int>("AppSettings:MaxAdvertsPerUser");

            if (numAdverts >= maxAdvertsPerUser)
            {
                throw new Exception($"Максимальное количество объявлений, которые вы можете выложить: {maxAdvertsPerUser}");
            }

            var advert = new Advert
            {
                Id = Guid.NewGuid(),
                User = user,
                Text = advertDto.Text,
                Rating = advertDto.Rating,
                TimeCreated = DateTime.UtcNow,
                ExpirationDate = advertDto.ExpirationDate,
                AdvertImages = new List<AdvertImage>()
            };           

            foreach (var image in images)
            {
                var filePath = _imageService.UploadImage(image);
                var advertImage = new AdvertImage
                {
                    FileName = image.FileName,
                    FileType = image.ContentType,
                    FilePath = filePath,
                    Advert = advert
                };
                advert.AdvertImages.Add(advertImage);
            }

            _dbContext.Adverts.Add(advert);
            await _dbContext.SaveChangesAsync();

            return advert.Id;
        }

        public async Task<AdvertDto> GetAdvertById(Guid id)
        {
            var advert = await _dbContext.Adverts
                .Include(a => a.User)
                .Include(a => a.AdvertImages)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (advert == null)
            {
                throw new Exception("Объявление не найдено");
            }

            var advertDto = _mapper.Map<AdvertDto>(advert);

            return advertDto;
        }
        public async Task DeleteAdvert(Guid advertId, Guid userId)
        {
            var advert = await _dbContext.Adverts
            .Include(a => a.User)
            .Include(a => a.AdvertImages)
            .SingleOrDefaultAsync(a => a.Id == advertId);

            if (advert == null)
            {
                throw new Exception("Объявление не найдено");
            }
            if (advert.UserId != userId)
            {
                throw new Exception("У вас нет прав удалять данное объявление");
            }
            foreach (var image in advert.AdvertImages)
            {
                _dbContext.AdvertImages.Remove(image);
                _imageService.DeleteImage(image.FilePath);
            }
            _dbContext.Adverts.Remove(advert);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAdvert(Guid advertId, Guid userId, string text, List<IFormFile> newImages, List<Guid> imagesToDelete)
        {
            var advert = await _dbContext.Adverts.Include(a => a.AdvertImages).FirstOrDefaultAsync(a => a.Id == advertId);

            if (advert == null)
            {
                throw new Exception("Объявление не найдено");
            }
            if (advert.UserId != userId) 
            {
                throw new Exception("У вас нет прав изменять данное объявление");
            }
            advert.Text = text;
            advert.ExpirationDate = DateTime.Now.AddDays(_config.GetValue<int>("Appsettings:ExpirationDate"));

            foreach (var imageToDeleteId in imagesToDelete)
            {
                var advertImage = advert.AdvertImages.FirstOrDefault(ai => ai.Id == imageToDeleteId);
                if (advertImage != null)
                {
                    _dbContext.AdvertImages.Remove(advertImage);
                    _imageService.DeleteImage(advertImage.FilePath);
                }
            }

            foreach (var newImage in newImages)
            {
                var filePath = _imageService.UploadImage(newImage);
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

    }
}