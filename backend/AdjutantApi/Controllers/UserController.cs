using System.Threading.Tasks;
using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdjutantApi.Controllers
{
    [ApiController, Route("[Controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<AdjutantUser> _userManager;

        public UserController(UserManager<AdjutantUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            // Wrap result in anonymous object to prevent leakage
            // of secret details in the given response
            return Ok(new {user.DiscordUsername, user.AvatarHash});
        }
    }
}