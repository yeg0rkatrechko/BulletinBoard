using Domain;
using Models.Dto;
using Services.Models;

namespace Services.DataContract
{
    public interface IAdvertRepository
    {
        Task<Guid> CreateAdvert(Advert advert);

        Task<IQueryable<Advert?>> GetAdvertById(Guid advertId);
        
        Task<IQueryable<Advert>>? GetPublishedAdverts(int pageNumber, int pageSize);
        
        Task UpdateAdvert(Advert advert);
        
        Task DeleteAdvert(Advert advert);
        
        Task<int> CountUserAdverts(Guid userId);
    }
}