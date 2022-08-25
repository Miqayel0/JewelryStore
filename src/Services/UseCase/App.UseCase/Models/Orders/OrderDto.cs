using App.Domain.Entities;
using App.Domain.Enums;
using App.UseCase.Common;

namespace App.UseCase.Models.Orders;

public class OrderDto : IAutoMap<Order, OrderDto>
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid UserId { get; set; }
    public int EmployeeId { get; set; }
    public decimal TotelPrice { get; set; } 
    public DateTime FinishDate { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto : IAutoMap<OrderItem, OrderItemDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int ExpectedTime { get; set; }
    public int CollectionId { get; set; }
    public int OrderId { get; set; }
}