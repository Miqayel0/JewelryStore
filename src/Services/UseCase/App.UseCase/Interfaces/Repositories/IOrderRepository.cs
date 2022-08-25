using App.Domain.Entities;
using App.Domain.Interfaces;

namespace App.UseCase.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order, int>
{
    Task<List<Order>> FindOrdersToReduceStartTimeAsync();
    Task<List<Order>> FindUserOrdersAsync(Guid userId);
    Task<Order> FindEmployeeFirstPendingOrderAsync(int employeeId);
}
