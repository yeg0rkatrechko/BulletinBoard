using Microsoft.AspNetCore.Mvc;
using Services;

namespace BulletinBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string name, bool isAdmin)
        {
            await _userService.CreateUser(name, isAdmin);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserPrivilege(Guid adminId, Guid userToChangeId, bool isAdmin)
        {
            _userService.ChangeUserPrivilege(adminId, userToChangeId, isAdmin);
            return NoContent();
        }

    }
}
