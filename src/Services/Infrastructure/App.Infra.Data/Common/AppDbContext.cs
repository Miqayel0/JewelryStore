using App.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Infra.Data.Common;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        return Database.CanConnectAsync(cancellationToken);
    }

    public Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        return Database.MigrateAsync(cancellationToken);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool commitOnSuccess = true)
    {
        return await base.SaveChangesAsync(commitOnSuccess, cancellationToken);
    }
}
