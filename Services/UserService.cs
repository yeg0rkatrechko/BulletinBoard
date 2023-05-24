using AutoMapper;
using Domain;
using Services.DataContract;
using Services.Models;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task CreateUser(string name, bool isAdmin)
        {
            var userExists = await _userRepository.GetUserByName(name);
            
            if (userExists!.Any())
            {
                throw new Exception("Пользователь с таким именем уже существует");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Admin = isAdmin
            };
            
            await _userRepository.CreateUser(user);
        }
        public async Task ChangeUserPrivilege(Guid adminId, Guid userToChangeId, bool isAdmin)
        {
            var admin = await _userRepository.GetUserById(adminId);
            var userToChange = await _userRepository.GetUserById(userToChangeId);
            if (admin == null || userToChange == null)
            {
                throw new Exception("Проверьте введенные ID");
            }
            var updatedUser = userToChange.FirstOrDefault();
            
            updatedUser!.Admin = isAdmin;

            await _userRepository.UpdateUser(updatedUser);
        }

        public async Task<List<AdvertDto>?> GetAdsByUser(Guid requestingUserId, Guid targetUserId)
        {
            var user = await _userRepository.GetUserById(targetUserId);

            if (user.Any() == false)
            {
                throw new Exception("Пользователь не найден");
            }

            if (user.FirstOrDefault()!.Adverts == null)
            {
                return null;
            }

            if (requestingUserId != targetUserId && user.FirstOrDefault()!.Adverts!.All(a => a.IsDraft))
            {
                return null;
            }

            var adverts = user.FirstOrDefault()!.Adverts!.ToList();

            if (requestingUserId != targetUserId)
            {
                adverts = adverts.Where(a => !a.IsDraft).ToList();
            }

            return _mapper.Map<List<AdvertDto>>(adverts);
        }

    }
}
