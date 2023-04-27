using BulletinBoard.ServiceModel;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Services;

namespace BulletinBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            await _userService.CreateUser(createUserRequest.Name, false);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserPrivilege(Guid adminId, Guid userToChangeId, bool isAdmin)
        {
            await _userService.ChangeUserPrivilege(adminId, userToChangeId, isAdmin);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAdsByUser(Guid requestingUserId, Guid targetUserId)
        {
            var response = await _userService.GetAdsByUser(requestingUserId, targetUserId);
            return Ok(response);
        }

    }
}
