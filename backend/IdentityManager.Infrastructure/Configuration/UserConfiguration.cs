using IdentityManager.Domain.Entities;
using IdentityManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityManager.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "coin_keeper");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasConversion(
                email => email.Value,
                value => new Email(value)
            )
            .HasMaxLength(100);

        builder.HasIndex(u => u.Email)
           .IsUnique();

        builder.Property(u => u.Password)
            .IsRequired()
            .HasConversion(
                password => password.Value,
                value => new Password(value)
            );

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();

        builder.Property(u => u.LastLogin)
            .IsRequired(false);

        builder.HasMany(u => u.Tokens)
            .WithOne()
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
