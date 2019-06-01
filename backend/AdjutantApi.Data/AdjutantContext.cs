using AdjutantApi.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdjutantApi.Data
{
    public class AdjutantContext : IdentityDbContext<AdjutantUser>
    {
        public DbSet<VerificationKey> VerificationKeys { get; set; }

        public AdjutantContext(DbContextOptions<AdjutantContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<VerificationKey>()
                .HasIndex(k => k.KeyValue)
                .IsUnique();
        }
    }
}
