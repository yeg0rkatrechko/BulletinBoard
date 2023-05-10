using AutoMapper;
using Dal;
using Domain;
using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly BulletinBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(BulletinBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task CreateUser(string name, bool isAdmin)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Name == name);
            if (userExists)
            {
                throw new Exception("Пользователь с таким именем уже существует");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Admin = isAdmin
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        public async Task ChangeUserPrivilege(Guid adminId, Guid userToChangeId, bool isAdmin)
        {
            var admin = await _dbContext.Users.SingleOrDefaultAsync(a => a.Id == adminId);
            var userToChange = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userToChangeId);
            if (admin == null || userToChange == null)
            {
                throw new Exception("Проверьте введенные ID");
            }
            userToChange.Admin = isAdmin;
        }

        public async Task<List<AdvertDto>> GetAdsByUser(Guid requestingUserId, Guid targetUserId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Adverts)
                .SingleOrDefaultAsync(u => u.Id == targetUserId);

            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            if (requestingUserId != targetUserId && !user.Adverts.Any(a => !a.IsDraft))
            {
                return new List<AdvertDto>();
            }

            var adverts = user.Adverts.ToList();

            if (requestingUserId != targetUserId)
            {
                adverts = adverts.Where(a => !a.IsDraft).ToList();
            }

            var advertsDto = _mapper.Map<List<AdvertDto>>(adverts);

            return advertsDto;
        }

    }
}
