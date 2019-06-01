using System;
using System.Collections.Generic;
using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdjutantApi.Identity
{
    public class AdjutantUserManager : UserManager<AdjutantUser>
    {
        // Long constructor :o
        public AdjutantUserManager(
            IUserStore<AdjutantUser> store,
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<AdjutantUser> passwordHasher,
            IEnumerable<IUserValidator<AdjutantUser>> userValidators,
            IEnumerable<IPasswordValidator<AdjutantUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services,
            ILogger<UserManager<AdjutantUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}