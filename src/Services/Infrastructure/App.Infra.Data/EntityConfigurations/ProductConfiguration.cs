using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("[NVARCHAR](200)")
            .IsRequired();

        builder.Property(e => e.Image)
            .HasColumnType("[VARCHAR](100)")
            .IsRequired(false);
    }
}
