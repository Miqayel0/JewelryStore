using App.Domain.Interfaces;
using App.Infra.DataSeed.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infra.DataSeed.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddDataSeeder(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, DataSeeder>();

        return services;
    }
}
