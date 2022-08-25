using App.UseCase.Models.Orders;

namespace App.UseCase.Interfaces.Commands;

public interface IOrderCommand
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
    Task FinishOrderAsync(int orderId);
}
