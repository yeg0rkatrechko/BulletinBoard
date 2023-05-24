using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bogus;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Models.Dto;
using Services.DataContract;
using Services.Models;
using Services.Options;

namespace Services
{
    public class AdvertService : IAdvertService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly AdvertOptions _options;
        private readonly IAdvertRepository _advertRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IReactionRepository _reactionRepository;

        public AdvertService(IMapper mapper, IOptions<AdvertOptions> options, IImageService imageService,
            IAdvertRepository advertRepository, IUserRepository userRepository, IImageRepository imageRepository,
            IReactionRepository reactionRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            ;
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _advertRepository = advertRepository ?? throw new ArgumentNullException(nameof(advertRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _reactionRepository = reactionRepository ?? throw new ArgumentNullException(nameof(reactionRepository));
        }

        public async Task<Guid> CreateAdvert(Guid userId, string? text, string? heading, List<IFormFile> images,
            bool isDraft)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user.Any() == false)
            {
                throw new Exception("Пользователя с данным id не существует");
            }

            var adsByUser = _userRepository.GetAdsByUser(userId);

            if (await _advertRepository.CountUserAdverts(userId) > _options.MaxAdvertsPerUser)
            {
                throw new Exception($"Вы не можете опубликовать более {_options.MaxAdvertsPerUser} объявлений");
            }

            var author = user.FirstOrDefault()!;

            var newAdvert = new Advert
            {
                Id = Guid.NewGuid(),
                User = author,
                Text = text,
                Heading = heading,
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
                    AdvertId = newAdvert.Id,
                    Advert = newAdvert
                };
                newAdvert.AdvertImages.Add(advertImage);
            }

            await _advertRepository.CreateAdvert(newAdvert);

            return newAdvert.Id;
        }

        public async Task<AdvertDto> GetAdvertById(Guid advertId, Guid? userId)
        {
            if (userId != null)
            {
                if (await _userRepository.GetUserById((Guid)userId) == null)
                {
                    throw new Exception("Пользователя с данным id не существует");
                }
            }
            var advert = await _advertRepository.GetAdvertById(advertId);

            if (advert.Any() == false || (advert.FirstOrDefault()!.IsDraft && advert.FirstOrDefault()!.User.Id != userId))
            {
                throw new Exception("Объявление не найдено");
            }

            var advertDto = advert.Select(a => new AdvertDto
            {
                UserName = a!.User.Name,
                Heading = a.Heading!,
                Text = a.Text!,
                TimeCreated = a.TimeCreated,
                ExpirationDate = a.ExpirationDate,
                AdvertImages = a.AdvertImages.Select(image => image.FilePath).ToList(),
                ReactionSum = a.AdvertReaction.Sum(ar => (int)ar.Reaction)
            }).FirstOrDefault();

            return advertDto!;
        }

        public async Task<List<AdvertDto>?> GetPublishedAdverts(int pageNumber, int pageSize)
        {
            var adverts = await _advertRepository.GetPublishedAdverts(pageNumber, pageSize);
            
            if (adverts.Any() == false)
            {
                return null;
            }

            var advertsDto = adverts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AdvertDto
                {
                    UserName = a.User.Name,
                    Heading = a.Heading!,
                    Text = a.Text!,
                    TimeCreated = a.TimeCreated,
                    ExpirationDate = a.ExpirationDate,
                    AdvertImages = a.AdvertImages.Select(image => image.FilePath).ToList(),
                    ReactionSum = a.AdvertReaction.Sum(ar => (int)ar.Reaction)
                }).ToList();

            return advertsDto;
        }

        public async Task<List<AdvertDto>?> SearchAdverts(int pageNumber, int pageSize, string? searchText,
            AdvertSortOrder sortOrder, DateTime? startDate, DateTime? endDate)
        {
            var query = await _advertRepository.GetPublishedAdverts(pageNumber, pageSize);

            if (query.Any() == false)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(a => a.Heading!.Contains(searchText) || a.Text!.Contains(searchText));
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.TimeCreated >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.TimeCreated <= endDate.Value);
            }

            switch (sortOrder)
            {
                case AdvertSortOrder.CreationDateAsc:
                    query = query.OrderBy(a => a.TimeCreated);
                    break;
                case AdvertSortOrder.CreationDateDesc:
                    query = query.OrderByDescending(a => a.TimeCreated);
                    break;
                case AdvertSortOrder.RatingAsc:
                    query = query.OrderBy(a =>
                        a.AdvertReaction.Count(r => r.Reaction == Reaction.Like) -
                        a.AdvertReaction.Count(r => r.Reaction == Reaction.Dislike));
                    break;
                case AdvertSortOrder.RatingDesc:
                    query = query.OrderByDescending(a =>
                        a.AdvertReaction.Count(r => r.Reaction == Reaction.Like) -
                        a.AdvertReaction.Count(r => r.Reaction == Reaction.Dislike));
                    break;
            }

            var sortedAdverts = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AdvertDto
                {
                    UserName = a.User.Name,
                    Heading = a.Heading!,
                    Text = a.Text!,
                    TimeCreated = a.TimeCreated,
                    ExpirationDate = a.ExpirationDate,
                    AdvertImages = a.AdvertImages.Select(image => image.FilePath).ToList(),
                    ReactionSum = a.AdvertReaction.Sum(ar => (int)ar.Reaction)
                }).ToList();

            return sortedAdverts;
        }

        public async Task DeleteAdvert(Guid advertId, Guid userId)
        {
            var advert = await _advertRepository.GetAdvertById(advertId);

            var user = await _userRepository.GetUserById(userId);

            if (advert.Any() == false || (advert.FirstOrDefault()!.IsDraft && advert.FirstOrDefault()!.User.Id != userId))
            {
                throw new Exception("Объявление не найдено");
            }

            if (user.Any() == false)
            {
                throw new Exception("Пользователь не найден");
            }

            if (user.FirstOrDefault()!.Admin && advert.FirstOrDefault()!.User.Id != userId)
            {
                throw new Exception("У вас нет прав удалять данное объявление");
            }

            await _advertRepository.DeleteAdvert(advert.FirstOrDefault()!);
        }

        public async Task UpdateAdvert(Guid advertId, Guid userId, string? text, string? heading, bool isDraft,
            List<IFormFile>? newImages, List<Guid>? imagesToDelete)
        {
            var advert = await _advertRepository.GetAdvertById(advertId);

            var advertToUpdate = advert.FirstOrDefault();

            if (advert.Any() == false)
            {
                throw new Exception("Объявление не найдено");
            }

            if (advertToUpdate!.User.Id != userId)
            {
                throw new Exception("У вас нет прав изменять данное объявление");
            }

            if (text != null)
            {
                advertToUpdate.Text = text;
            }

            if (heading != null)
            {
                advertToUpdate.Heading = heading;
            }

            if (advertToUpdate.IsDraft)
            {
                advertToUpdate.IsDraft = isDraft;
            }

            advertToUpdate.ExpirationDate = DateTime.Now.AddDays(_options.ExpirationDate);

            if (advertToUpdate.AdvertImages != null && imagesToDelete != null)
            {
                foreach (var imageToDeleteId in imagesToDelete)
                {
                    var advertImagePath = advertToUpdate.AdvertImages.FirstOrDefault(ai => ai.Id == imageToDeleteId)?.FilePath;
                    if (advertImagePath != null)
                    {
                        await _imageRepository.DeleteImage(advertImagePath);
                    }
                }
            }

            if (newImages != null)
                foreach (var newImage in newImages)
                {
                    var filePath = await _imageService.UploadImage(newImage);
                    var advertImage = new AdvertImage
                    {
                        FileName = newImage.FileName,
                        FileType = newImage.ContentType,
                        FilePath = filePath,
                        AdvertId = advertId,
                        Advert = advertToUpdate
                    };
                    advertToUpdate.AdvertImages?.Add(advertImage);
                }

            await _advertRepository.UpdateAdvert(advertToUpdate);
        }

        public async Task ReactToAdvert(Guid userId, Guid advertId, Reaction reaction)
        {
            var advert = await _advertRepository.GetAdvertById(advertId);

            if (advert.Any() == false)
            {
                throw new Exception("Объявление не найдено");
            }

            var advertToReact = advert.FirstOrDefault()!;

            if (advertToReact.AdvertReaction != null)
            {
                var existingReaction =
                    advertToReact.AdvertReaction.FirstOrDefault(r => r.UserId == userId);
                if (existingReaction != null)
                {
                    existingReaction.Reaction = reaction;
                    var updatedReaction = existingReaction;
                    await _reactionRepository.UpdateReaction(updatedReaction);
                }
            }
            else
            {
                var newReaction = new AdvertReaction
                {
                    UserId = userId,
                    AdvertId = advertId,
                    Reaction = reaction
                };
                await _reactionRepository.CreateReaction(newReaction);
            }
        }
    }
}