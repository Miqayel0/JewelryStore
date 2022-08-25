using App.Domain.Entities;
using App.Domain.Enums;
using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Hubs;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Orders;
using AutoMapper;
using Common.Exceptions;

namespace App.UseCase.Command.V1;

public class OrderCommand : IOrderCommand
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly INotificationHubContextAccessor _notificationHubContextAccessor;
    private readonly IUserSessionCommand _userSessionCommand;
    private readonly IMapper _mapper;

    public OrderCommand(
        IOrderRepository orderRepository,
        IEmployeeRepository employeeRepository,
        ICollectionRepository collectionRepository,
        INotificationHubContextAccessor notificationHubContextAccessor,
        IUserSessionCommand userSessionCommand,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _employeeRepository = employeeRepository;
        _collectionRepository = collectionRepository;
        _notificationHubContextAccessor = notificationHubContextAccessor;
        _userSessionCommand = userSessionCommand;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
    {
        var userId = _userSessionCommand.GetId();
        var collection = await _collectionRepository.FindByIdAsync(dto.CollectionId)
            ?? throw new NotFoundException("Collection not found");
        var employee = await _employeeRepository.FindEmployeeToAssignAsync();
        var order = await _orderRepository.AddAsync(new Order
        {
            Status = employee.Assigned ? OrderStatus.Pending : OrderStatus.Started,
            StartDate = employee.FinishDate,
            CreationDate = DateTime.UtcNow,
            EmployeeId = employee.Id,
            UserId = userId,
        });
        order.Items.Add(new OrderItem
        {
            CollectionId = collection.Id,
            Name = collection.Name,
            Price = collection.Price,
            ExpectedTime = collection.ExpectedTime,
        });
        await _orderRepository.SaveChangesAsync();
        return _mapper.Map<OrderDto>(order);
    }

    public async Task FinishOrderAsync(int orderId)
    {
        var order = await _orderRepository.FindByIdAsync(orderId)
            ?? throw new NotFoundException("Order not found");
        if (order.Status != OrderStatus.Started)
            throw new BadRequestException("Order status must be started");

        order.Status = OrderStatus.Ready;
        _orderRepository.Update(order);
        
        var employeeFirstPendingOrder = await _orderRepository.FindEmployeeFirstPendingOrderAsync(order.EmployeeId);
        if (employeeFirstPendingOrder != null)
        {
            employeeFirstPendingOrder.Status = OrderStatus.Started;
            _orderRepository.Update(employeeFirstPendingOrder);
        }

        await _orderRepository.SaveChangesAsync();
        await _notificationHubContextAccessor.NotifyOrderStatusChangeAsync(order);

        if (employeeFirstPendingOrder != null)
        {
            await _notificationHubContextAccessor.NotifyOrderStatusChangeAsync(employeeFirstPendingOrder);
        }
    }
}
