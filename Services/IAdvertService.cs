using Microsoft.AspNetCore.Http;
using Models.DbModels;
using Models.Dto;

namespace Services
{
    public interface IAdvertService
    {
        Task<Guid> CreateAdvert(Guid userId, string text, List<IFormFile> images, bool isDraft);
        Task<AdvertDto> GetAdvertById(Guid advertId, Guid userId);
        Task<IList<AdvertDto>> GetAllPublishedAdverts();
        Task<List<AdvertDto>> SearchAdverts(string? searchText, AdvertSortOrder sortOrder, DateTime? startDate, DateTime? endDate);
        Task DeleteAdvert(Guid advertId, Guid userId);
        Task UpdateAdvert(Guid advertId, Guid userId, string text, bool isDraft, List<IFormFile> newImages, List<Guid> imagesToDelete);
        Task ReactToAdvert(Guid userId, Guid advertId, Reaction reaction);
    }
}
