using App.Domain.Interfaces;

namespace AppAPI.Extensions;

public static class DbExtension
{
    public static async Task MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var databaseExists = await context.CanConnectAsync();
        if (!databaseExists)
        {
            await context.MigrateAsync();
        }

        var seeder = scope.ServiceProvider.GetService<IDataSeeder>();
        if (seeder == null)
            return;
        await seeder.SeedAsync();
    }
}
