using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdjutantApi.Data.Models;
using AdjutantApi.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace AdjutantApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<AdjutantUser> _signInManager;
        private readonly AdjutantUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(SignInManager<AdjutantUser> signInManager, AdjutantUserManager userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
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
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded) return Redirect(_configuration["FrontendOAuthCallback"]);

            var username = info.Principal.FindFirstValue(ClaimTypes.Name);
            var usernameWithDiscriminator = $"{username}#{info.Principal.FindFirstValue(ClaimTypes.Surname)}";

            var newUser = new AdjutantUser
            {
                UserName = username,
                DiscordUsername = usernameWithDiscriminator,
                AvatarHash = info.Principal.FindFirstValue(ClaimTypes.UserData)
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
            await _userManager.AddLoginAsync(newUser, info); // User id claim type: http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
            await _signInManager.SignInAsync(newUser, isPersistent: false);

            if (_signInManager.IsSignedIn(User))
            {
                return Redirect(_configuration["FrontendOAuthCallback"]);
            }

            return BadRequest(new { Error = "Something went wrong during the authentication process!" });
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Successfully logged out!");
        }
    }

}