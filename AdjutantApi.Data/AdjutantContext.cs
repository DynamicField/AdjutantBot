using AdjutantApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdjutantApi.Data
{
    public class AdjutantContext : DbContext
    {
        public DbSet<VerificationKey> VerificationKeys { get; set; }
        public DbSet<DiscordAccount> DiscordAccounts { get; set; }
        
        public AdjutantContext(DbContextOptions<AdjutantContext> options) : base(options){}
    }
}
