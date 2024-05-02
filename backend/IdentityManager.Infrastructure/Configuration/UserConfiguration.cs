using IdentityManager.Domain.Entities;
using IdentityManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityManager.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    const int usernameMaxLength = 50;
    const int emailMaxLength = 255;
    const int passwordMaxLength = 255;

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "identity");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(usernameMaxLength);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasConversion(
                email => email.Value,
                value => new Email(value)
            )
            .HasMaxLength(emailMaxLength);

        builder.HasIndex(u => u.Email)
           .IsUnique();

        builder.Property(u => u.Password)
            .IsRequired()
            .HasConversion(
                password => password.Value,
                value => new Password(value)
            )
            .HasMaxLength(passwordMaxLength);

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
