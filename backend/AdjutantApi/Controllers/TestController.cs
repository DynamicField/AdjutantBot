using System.Threading.Tasks;
using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdjutantApi.Controllers
{
    [ApiController, Route("[Controller]")]
    public class TestController : Controller
    {
        private UserManager<AdjutantUser> _userManager;
        public TestController(UserManager<AdjutantUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return Ok(user);
        }
    }
}