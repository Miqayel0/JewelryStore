using App.SignalRHub.Services;
using App.UseCase.Interfaces.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.SignalRHub.Extensions
{
    public static class DependencyExtensions
    {
        public static void AddSignalRHubs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR()
                .AddStackExchangeRedis(configuration.GetConnectionString("RedisConnection"));

            services.AddScoped<INotificationHubContextAccessor, NotificationHubContextAccessor>();
        }
    }
}