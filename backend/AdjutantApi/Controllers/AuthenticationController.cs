using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AdjutantApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<AdjutantUser> _signInManager;
        private readonly UserManager<AdjutantUser> _userManager;
        
        public AuthenticationController(SignInManager<AdjutantUser> signInManager, UserManager<AdjutantUser> userManager)
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
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded) return Redirect("http://localhost:8080/test");

            var username = info.Principal.FindFirstValue(ClaimTypes.Name);
            var usernameWithDiscriminator = $"{username}#" +
                        $"{info.Principal.FindFirstValue(ClaimTypes.Surname)}";
            
            var newUser = new AdjutantUser {
                UserName = username,
                DiscordUsername = usernameWithDiscriminator,
                AvatarHash = info.Principal.FindFirstValue(ClaimTypes.UserData)
            };
            
            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                throw new Exception(createResult.Errors.Select(e => e.Description)
                    .Aggregate((errors, error) => $"{errors}, {error}"));
            }

            await _userManager.AddLoginAsync(newUser, info);
            var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
            await _userManager.AddClaimsAsync(newUser, newUserClaims);
            await _signInManager.SignInAsync(newUser, isPersistent: false);

            if (_signInManager.IsSignedIn(User))
            {
                return Ok(HttpContext.Response.Cookies);
            }

            return BadRequest(new {Error = "Something went wrong during the authentication process!"});
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