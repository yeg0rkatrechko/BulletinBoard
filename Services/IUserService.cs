namespace Services
{
    public interface IUserService
    {
        Task CreateUser(string name, bool isAdmin);
        Task ChangeUserPrivilege(Guid adminId, Guid userToChangeId, bool isAdmin);
    }
}
