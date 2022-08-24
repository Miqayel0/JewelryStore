using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class Product : IAggregateRoot<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public List<CollectionProduct> Collections { get; set; } = new();
}
