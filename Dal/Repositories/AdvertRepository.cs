using Domain;
using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Services.DataContract;

namespace Dal.Repositories;

public class AdvertRepository : IAdvertRepository
{
    private readonly BulletinBoardDbContext _dbContext;

    public AdvertRepository(BulletinBoardDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Guid> CreateAdvert(Advert advert)
    {
        _dbContext.Adverts.Add(advert);
        await _dbContext.SaveChangesAsync();

        return advert.Id;
    }

    public async Task<IQueryable<Advert?>>  GetAdvertById(Guid advertId)
    {
        return _dbContext.Adverts
            .Where(a => a.Id == advertId)
            .Include(a => a.AdvertImages)
            .Include(a => a.User)
            .Include(a => a.AdvertReaction);
    }

    public Task<IQueryable<Advert>>? GetPublishedAdverts(int pageNumber, int pageSize)
    {
        return Task.FromResult<IQueryable<Advert>>(_dbContext.Adverts
            .Where(a => a.IsDraft == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(a => a.User)
            .Include(a => a.AdvertImages)
            .Include(a => a.AdvertReaction));
    }

    public async Task UpdateAdvert(Advert advert)
    {
        _dbContext.Adverts.Update(advert);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAdvert(Advert advert)
    {
        _dbContext.Adverts.Remove(advert);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CountUserAdverts(Guid userId)
    {
        return await _dbContext.Adverts.CountAsync(a => a.UserId == userId);
    }
}