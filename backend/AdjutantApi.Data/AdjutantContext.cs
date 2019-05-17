using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdjutantApi.Data
{
    public class AdjutantContext : IdentityDbContext
    {
        public DbSet<VerificationKey> VerificationKeys { get; set; }
        public DbSet<DiscordAccount> DiscordAccounts { get; set; }
        
        public AdjutantContext(DbContextOptions<AdjutantContext> options) : base(options){}
    }
}
