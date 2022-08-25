using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Infra.Data.Common;
using App.Infra.DataSeed.DataProviders;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.DataSeed.Seeders;

public class DataSeeder : IDataSeeder
{
    private readonly AppDbContext _context;

    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedAsync(new JsonDataProvider<Material>());
        await SeedAsync(new JsonDataProvider<Product>());
        await SeedAsync(new JsonDataProvider<Employee>());
        await SeedAsync(new JsonDataProvider<Collection>());
        await SeedAsync(new JsonDataProvider<CollectionProduct>());

        await SeedAsync(new ScriptDataProvider("sp_getEmployeeToAssign"));
    }

    private async Task SeedAsync(IScriptDataProvider scriptDataProvider)
    {
        var sql = scriptDataProvider.GetScriptSql();
        await _context.Database.ExecuteSqlRawAsync(sql);
    }

    private async Task SeedAsync<TEntity>(IJsonDataProvider<TEntity> jsonDataProvider) where TEntity : class
    {
        if (await _context.Set<TEntity>().AnyAsync())
        {
            return;
        }

        var tableName = typeof(TEntity).Name;
        var data = jsonDataProvider.GetData();
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            await _context.AddRangeAsync(data);
            await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} ON;");
            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} OFF");
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }
}
