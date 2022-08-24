using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable(nameof(Material));
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("[NVARCHAR](200)")
            .IsRequired();

        builder.HasMany(e => e.Collections)
            .WithOne(e => e.Material)
            .HasForeignKey(e => e.MaterialId)
            .IsRequired();
    }
}
