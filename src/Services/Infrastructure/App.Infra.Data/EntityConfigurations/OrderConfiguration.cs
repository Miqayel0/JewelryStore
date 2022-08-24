using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));
        builder.HasKey(e => e.Id);

        builder.Ignore(e => e.TotelPrice);

        builder.HasOne(e => e.User)
            .WithMany(e => e.Orders)
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.HasOne(e => e.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.EmployeeId)
            .IsRequired();

        builder.HasMany(e => e.Items)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId)
            .IsRequired();
    }
}
