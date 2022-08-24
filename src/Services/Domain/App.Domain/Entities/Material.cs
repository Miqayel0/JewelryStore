using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class Material : IAggregateRoot<int>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Collection> Collections { get; set; } = new();
}
