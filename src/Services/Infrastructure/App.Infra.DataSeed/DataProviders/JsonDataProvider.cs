using App.Infra.DataSeed.Seeders;
using Common.Extensions;
using System.Text.Json;

namespace App.Infra.DataSeed.DataProviders;

public class JsonDataProvider<TEntity> : IJsonDataProvider<TEntity>
{
    public TEntity[] GetData()
    {
        var filename = $"{typeof(TEntity).Name}.json";
        var content = filename.ReadEmbeddedResource<DataSeeder>();
        var list = JsonSerializer.Deserialize<TEntity[]>(content);
        return list?.ToArray() ?? Array.Empty<TEntity>();
    }
}
