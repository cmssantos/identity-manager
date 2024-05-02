using IdentityManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityManager.Infrastructure.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    const int tokenMaxLength = 255;

    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens", "identity");

        builder.HasKey(ut => ut.Token);

        builder.Property(ut => ut.Token)
            .HasMaxLength(tokenMaxLength)
            .IsRequired();

        builder.Property(ut => ut.Type)
            .IsRequired();

        builder.Property(ut => ut.ExpirationDate)
            .IsRequired();

        builder.Property(ut => ut.IsUsed)
            .IsRequired();

        builder.Property(ut => ut.IsRevoked)
            .IsRequired();

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.Tokens)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
