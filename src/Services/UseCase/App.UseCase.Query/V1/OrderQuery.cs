using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Queries;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Orders;
using AutoMapper;

namespace App.UseCase.Query.V1;

internal class OrderQuery : IOrderQuery
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserSessionCommand _userSessionCommand;
    private readonly IMapper _mapper;

    public OrderQuery(
        IOrderRepository orderRepository,
        IUserSessionCommand userSessionCommand,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _userSessionCommand = userSessionCommand;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> GetUsersOrdersAsync(Guid? userId = null)
    {
        userId ??= _userSessionCommand.GetId();
        var orders = await _orderRepository.FindUserOrdersAsync(userId.Value);
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
