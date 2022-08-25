using App.UseCase.Models.Orders;

namespace App.UseCase.Interfaces.Queries;

public interface IOrderQuery
{
    Task<List<OrderDto>> GetUsersOrdersAsync(Guid? userId = null);
}
