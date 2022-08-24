using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("[NVARCHAR](200)")
            .IsRequired();

        builder.Property(e => e.Price)
            .HasConversion<decimal>()
            .HasColumnType("DECIMAL(19,9)").IsRequired();
    }
}
