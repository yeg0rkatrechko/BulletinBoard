using Domain;

namespace Services.DataContract;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task<IQueryable<Advert?>> GetAdsByUser(Guid userId);
    Task<IQueryable<User?>> GetUserById(Guid userId);
    Task<IQueryable<User?>> GetUserByName(string userName);
}