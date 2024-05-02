using IdentityManager.Domain.Entities;
using IdentityManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");

        base.OnModelCreating(modelBuilder);

        // Apply the configuration for the User entity
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
    }
}
