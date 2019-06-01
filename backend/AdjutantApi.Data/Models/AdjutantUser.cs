using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdjutantApi.Data.Models
{
    public class AdjutantUser : IdentityUser
    {
        public VerificationKey BoundKey { get; set; }
        public string AvatarHash { get; set; }

        [StringLength(37)] // Discord username: 32 characters, # 1 character, 4 numbers discriminator = 37
        public string DiscordUsername { get; set; }
    }
}