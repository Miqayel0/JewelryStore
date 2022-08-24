using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class CollectionProduct : IEntity<int>
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CollectionId { get; set; }
    public Product Product { get; set; }
    public Collection Collection { get; set; }
}
