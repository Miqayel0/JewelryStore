using App.Domain.Interfaces;
using App.Infra.Data.Common;
using App.Infra.Data.Repositories;
using App.UseCase.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Infra.Data.Exntensions;

public static class DependencyExtension
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IUnitOfWork, AppDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICollectionRepository, CollectionRepository>();

        return services;
    }
}
