using Microsoft.AspNetCore.Mvc;
using Models;
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
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            await _userService.CreateUser(userDto);
            return NoContent();
        }

    }
}
