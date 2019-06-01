using System.Threading.Tasks;
using AdjutantApi.Data.Models;
using AdjutantApi.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdjutantApi.Controllers
{
    [ApiController, Route("[Controller]")]
    public class UserController : Controller
    {
        private readonly AdjutantUserManager _userManager;

        public UserController(AdjutantUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("me")]
        [HttpGet("@me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            // Wrap result in anonymous object to prevent leakage
            // of secret details in the given response
            return Ok(new { user.DiscordUsername, user.AvatarHash });
        }
    }
}