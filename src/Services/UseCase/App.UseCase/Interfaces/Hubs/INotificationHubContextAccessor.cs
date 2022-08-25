using App.Domain.Entities;

namespace App.UseCase.Interfaces.Hubs;

public interface INotificationHubContextAccessor
{
    Task NotifyOrderStatusChangeAsync(Order order);
}
