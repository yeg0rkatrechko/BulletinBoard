using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace Services
{
    public class UserService
    {
        private readonly BulletinBoardDbContext _dbContext;
        public UserService(BulletinBoardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateUser(string name, bool isAdmin)
        {
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
    }
}
