using App.UseCase.Interfaces.Queries;
using App.UseCase.Query.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.UseCase.Query.Extensions;

public static class DependencyExtension
{
    public static IServiceCollection AddQueryUseCases(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUserInfoQuery, UserInfoQuery>();
        services.AddScoped<IOrderQuery, OrderQuery>();

        return services;
    }
}
