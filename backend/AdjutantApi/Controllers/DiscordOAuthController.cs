using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdjutantApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class DiscordOAuthController : Controller
    {
        [HttpGet("oauth-redirect")]
        public async Task<IActionResult> Index([FromQuery(Name = "code")]string token, [FromQuery(Name = "state")]string csrfToken)
        {
            var apiEndpoint = "https://discordapp.com/api/v6";
            var client = HttpClientFactory.Create();

            var requestBody = new Dictionary<string, string>();
            requestBody.Add("client_id", Environment.GetEnvironmentVariable("CLIENT_ID"));
            requestBody.Add("client_secret", Environment.GetEnvironmentVariable("CLIENT_SECRET"));
            requestBody.Add("grant_type", "authorization_code");
            requestBody.Add("code", token);
            requestBody.Add("redirect_uri", "http://localhost:8080");
            requestBody.Add("scope", "identify guilds");
            
            
            var content = new FormUrlEncodedContent(requestBody);
            var result = await client.PostAsync($"{apiEndpoint}/oauth2/token", content);

            if (!result.IsSuccessStatusCode)
            {
                BadRequest("Could not fetch authorization token from discord!");
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
            
            Console.WriteLine(await result.Content.ReadAsStringAsync());
            return Ok();
        }
    }
}