using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

internal class CollectionProductConfiguration : IEntityTypeConfiguration<CollectionProduct>
{
    public void Configure(EntityTypeBuilder<CollectionProduct> builder)
    {
        builder.ToTable(nameof(CollectionProduct));
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Product)
            .WithMany(e => e.Collections)
            .HasForeignKey(e => e.ProductId);

        builder.HasOne(e => e.Collection)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.CollectionId);
    }
}
