using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdjutantApi.Controllers
{
    [ApiController, Route("[Controller]")]
    public class TestController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        public TestController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return Ok(user);
        }
    }
}