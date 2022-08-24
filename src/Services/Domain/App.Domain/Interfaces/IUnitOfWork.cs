namespace App.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool commitOnSuccess = true);
}
