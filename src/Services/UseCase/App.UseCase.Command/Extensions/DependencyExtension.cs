using App.UseCase.Command.V1;
using App.UseCase.Interfaces.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.UseCase.Command.Extensions;

public static class DependencyExtension
{
    public static IServiceCollection AddCommandUseCases(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUserAccountCommand, UserAccountCommand>();
        services.AddScoped<ITokenStoreCommand, TokenStoreCommand>();
        services.AddScoped<ITokenValidatorCommand, TokenValidatorCommand>();
        services.AddScoped<IOrderCommand, OrderCommand>();
        services.AddScoped<IEmployeeCommand, EmployeeCommand>();

        services.AddTransient<IUserSessionCommand, UserSessionCommand>();

        return services;
    }
}
