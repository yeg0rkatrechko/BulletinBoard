using System.Data.Entity;
using Domain;
using Services.DataContract;

namespace Dal.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BulletinBoardDbContext _dbContext;

    public UserRepository(BulletinBoardDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IQueryable<User?>> GetUserById(Guid userId)
    {
        return _dbContext.Users
            .Include(u => u.Adverts)
            .Where(u => u.Id == userId);
    }

    public async Task<IQueryable<User?>> GetUserByName(string userName)
    {
        var user = _dbContext.Users.Where(u => u.Name == userName);
        return user;
    }

    public async Task UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IQueryable<Advert?>> GetAdsByUser(Guid userId)
    {
        var adverts = _dbContext.Adverts.Where(a => a.UserId == userId);
        return adverts;
    }
    
    
}