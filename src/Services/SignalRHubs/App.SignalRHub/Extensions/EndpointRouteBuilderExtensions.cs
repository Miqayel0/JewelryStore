using App.SignalRHub.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace App.SignalRHub.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<NotificationHub>("/hubs/notification")
                .RequireAuthorization();
            return endpoints;
        }
    }
}