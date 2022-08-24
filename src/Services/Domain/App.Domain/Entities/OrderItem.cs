using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class OrderItem : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int ExpectedTime { get; set; }
    public int CollectionId { get; set; }
    public int OrderId { get; set; }
    public Collection Collection { get; set; }
    public Order Order { get; set; }
}
