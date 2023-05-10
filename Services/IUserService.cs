using Models.Dto;
using Services.Models;

namespace Services
{
    public interface IUserService
    {
        Task CreateUser(string name, bool isAdmin);
        Task ChangeUserPrivilege(Guid adminId, Guid userToChangeId, bool isAdmin);
        Task<List<AdvertDto>> GetAdsByUser(Guid requestingUserId, Guid targetUserId);
    }
}
