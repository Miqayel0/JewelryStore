using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Queries;
using App.UseCase.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers.V1;

public class OrderController : ApiController
{
    private readonly IOrderQuery _orderQuery;
    private readonly IOrderCommand _orderCommand;

    public OrderController(
        IOrderQuery orderQuery,
        IOrderCommand orderCommand)
    {
        _orderQuery = orderQuery;
        _orderCommand = orderCommand;
    }

    [HttpGet]
    public Task<List<OrderDto>> GetUserOrders()
    {
        return _orderQuery.GetUsersOrdersAsync();
    }

    [HttpPost]
    public Task<OrderDto> CreateOrder(CreateOrderDto dto)
    {
        return _orderCommand.CreateOrderAsync(dto);
    }

    [HttpPut("{orderId:int}/Finish")]
    [AllowAnonymous]
    public async Task<NoContentResult> FinishOrder(int orderId)
    {
        await _orderCommand.FinishOrderAsync(orderId);
        return NoContent();
    }
}
