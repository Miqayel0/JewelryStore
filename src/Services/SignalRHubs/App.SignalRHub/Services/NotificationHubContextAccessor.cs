using App.Domain.Entities;
using App.SignalRHub.Hubs;
using App.UseCase.Interfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace App.SignalRHub.Services;

internal class NotificationHubContextAccessor : INotificationHubContextAccessor
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubContextAccessor(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyOrderStatusChangeAsync(Order order)
    {
        return _hubContext.Clients.User(order.UserId.ToString())
            .SendAsync("OrderStatusChange", order);
    }
}
