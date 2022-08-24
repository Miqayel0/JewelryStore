using App.Domain.Entities;
using App.Domain.Enums;
using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Orders;
using Common.Exceptions;

namespace App.UseCase.Command.V1;

public class OrderCommand : IOrderCommand
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICollectionRepository _collectionRepository;

    public OrderCommand(IOrderRepository orderRepository, ICollectionRepository collectionRepository)
    {
        _orderRepository = orderRepository;
        _collectionRepository = collectionRepository;
    }

    public async Task<int> CreateOrderAsync(OrderDto dto)
    {
        var collection = await _collectionRepository.FindByIdAsync(dto.CollectionId)
            ?? throw new NotFoundException("Collection not found");

        var order = await _orderRepository.AddAsync(new Order
        {
            Status = OrderStatus.Pending,
        });
        order.Items.Add(new OrderItem
        {
            CollectionId = collection.Id,
            Name = collection.Name,
            Price = collection.Price,
            //ExpectedTime
        });
        await _orderRepository.SaveChangesAsync();
        return order.Id;
    }
}
