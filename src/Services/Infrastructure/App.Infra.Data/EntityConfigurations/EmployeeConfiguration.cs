using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.EntityConfigurations;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(nameof(Employee));
        builder.HasIndex(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("NVARCHAR(200)")
            .IsRequired();
    }
}
