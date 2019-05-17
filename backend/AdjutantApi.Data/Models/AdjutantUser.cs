using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdjutantApi.Data.Models
{
    public class AdjutantUser : IdentityUser
    {
        public VerificationKey BoundKey { get; set; }
        public string AvatarHash { get; set; }
        public string DiscordUsername { get; set; }
    }
}