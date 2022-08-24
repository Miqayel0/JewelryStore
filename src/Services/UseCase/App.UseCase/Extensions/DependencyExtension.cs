using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using App.UseCase.Mappings;

namespace App.UseCase.Extensions;

public static class DependencyExtension
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(new Assembly[] { Assembly.GetAssembly(typeof(MappingProfile)) }, ServiceLifetime.Scoped);

        return services;
    }
}
