using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class Collection : IAggregateRoot<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int ExpectedTime { get; set; }
    public int MaterialId { get; set; }
    public Material Material { get; set; }
    public List<CollectionProduct> Products { get; set; } = new();
    public List<OrderItem> OrderItems { get; set; } = new();
}
