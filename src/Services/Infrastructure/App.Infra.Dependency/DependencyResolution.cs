using App.Infra.Data.Exntensions;
using App.Infra.DataSeed.Extensions;
using App.SignalRHub.Extensions;
using App.UseCase.Command.Extensions;
using App.UseCase.Extensions;
using App.UseCase.Query.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Infra.Dependency;

public static class DependencyResolution
{
    public static IServiceCollection AddCustomDependencies(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddInfra(configuration, env);
        services.AddCommandUseCases(configuration);
        services.AddQueryUseCases(configuration);
        services.AddUseCases(configuration);
        services.AddDataSeeder();
        services.AddSignalRHubs(configuration);

        return services;
    }
}
