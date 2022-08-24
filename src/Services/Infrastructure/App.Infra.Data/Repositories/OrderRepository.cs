using App.Domain.Entities;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;

namespace App.Infra.Data.Repositories;

public class OrderRepository : RepositoryBase<Order, int>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}
