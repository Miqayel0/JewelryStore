using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.UserId)
            .IncludeProperties(e => new
            {
                e.AccessTokenHash,
                e.AccessTokenExpireDate,
                e.RefreshTokenHash,
                e.RefreshTokenHashSource
            });

        builder.HasOne(e => e.User)
            .WithMany(e => e.UserTokens)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.UserId).IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.AccessTokenHash)
            .HasColumnType("varchar(3000)")
            .IsRequired();

        builder.Property(e => e.RefreshTokenHash)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(e => e.RefreshTokenHashSource)
            .HasColumnType("varchar(3000)")
            .IsRequired(false);
    }
}
