using App.Domain.Enums;
using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class Order : IAggregateRoot<int>
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public Guid UserId { get; set; }
    public int EmployeeId { get; set; }
    public User User { get; set; }
    public Employee Employee { get; set; }
    public List<OrderItem> Items { get; set; } = new();

    public decimal TotelPrice => Items.Sum(item => item.Price);
}
