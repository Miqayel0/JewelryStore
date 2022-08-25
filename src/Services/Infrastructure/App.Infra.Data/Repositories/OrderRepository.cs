using App.Domain.Entities;
using App.Domain.Enums;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data.Repositories;

public class OrderRepository : RepositoryBase<Order, int>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Order>> FindOrdersToReduceStartTimeAsync()
    {
        var query = await DbConnection.QueryAsync<Order>(@"
            SELECT *
            FROM dbo.[Order] o
            WHERE o.[Status] = 1 AND o.[EmployeeId] IN (
	            SELECT o.[EmployeeId]
	            FROM dbo.[Order] o
	            WHERE o.[StartDate] = (
		            SELECT MIN(o.[StartDate])
		            FROM dbo.[Order] o
		            WHERE o.[Status] = 1 AND o.[StartDate] > GETUTCDATE()
	            )
            )
            ORDER BY o.[StartDate]
            ");
        return query.ToList();
    }

    public Task<List<Order>> FindUserOrdersAsync(Guid userId)
    {
        return Filter(e => e.UserId == userId)
            .Include(e => e.Items)
            .ToListAsync();
    }

    public Task<Order> FindEmployeeFirstPendingOrderAsync(int employeeId)
    {
        return Filter(e => e.EmployeeId == employeeId && e.Status == OrderStatus.Pending)
            .OrderBy(e => e.Id)
            .FirstOrDefaultAsync();
    }
}
