using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

internal class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder.ToTable(nameof(Collection));
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("[NVARCHAR](200)")
            .IsRequired();

        builder.Property(e => e.Price)
            .HasConversion<decimal>()
            .HasColumnType("DECIMAL(19,9)").IsRequired();

        builder.HasMany(e => e.OrderItems)
            .WithOne(e => e.Collection)
            .HasForeignKey(e => e.CollectionId)
            .IsRequired();
    }
}
