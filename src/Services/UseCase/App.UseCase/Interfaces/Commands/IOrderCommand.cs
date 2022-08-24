using App.UseCase.Models.Orders;

namespace App.UseCase.Interfaces.Commands;

public interface IOrderCommand
{
    Task<int> CreateOrderAsync(OrderDto dto);
}
