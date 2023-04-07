using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace Services
{
    public class UserService
    {
        private readonly BulletinBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserService(IMapper mapper, BulletinBoardDbContext dbContext, IConfiguration config)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _config = config;
        }
        public async Task<Guid> CreateUser(UserDto userDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Admin = userDto.Admin
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user.Id;
        }
    }
}
