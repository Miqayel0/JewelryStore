using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class Employee : IAggregateRoot<int>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Order> Orders { get; set; } = new();
}
