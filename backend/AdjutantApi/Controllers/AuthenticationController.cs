using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AdjutantApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public AuthenticationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet(nameof(SignInWithDiscord))]
        public IActionResult SignInWithDiscord()
        {
            // This tells the asp.net core middleware to redirect to HandleExternalLogin after handling the /signin-discord callback
            var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Discord", Url.Action(nameof(HandleExternalLogin)));
            return Challenge(authenticationProperties, "Discord");
        }
        
        [HttpGet("OAuthCallback")]
        public async Task<IActionResult> HandleExternalLogin()
        {
            // TODO: how do i handle state with the discord oauth package.exe
            var info = await _signInManager.GetExternalLoginInfoAsync();

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded) return Redirect("http://localhost:4200");
            var email = info.Principal.FindFirstValue(ClaimTypes.Name);
            var newUser = new IdentityUser {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
                throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

            await _userManager.AddLoginAsync(newUser, info);
            var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
            await _userManager.AddClaimsAsync(newUser, newUserClaims);
            await _signInManager.SignInAsync(newUser, isPersistent: false);

            return Ok("bro lemme in");
        }
    }
    
}