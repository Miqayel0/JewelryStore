using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Key);
        builder.HasIndex(e => e.Id).IsUnique();
        builder.HasIndex(e => e.Username).IsUnique();

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(e => e.Username)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(e => e.PasswordHash)
            .HasColumnType("varchar(4000)")
            .IsRequired();

        builder.Property(e => e.SecurityStamp)
            .HasColumnType("varchar(4000)")
            .IsRequired();

        builder.Property(e => e.IsBlocked)
            .HasColumnType("bit")
            .IsRequired();

        builder.Property(e => e.CreationDate)
            .IsRequired();
    }
}
