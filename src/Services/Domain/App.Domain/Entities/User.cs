using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class User : IAggregateRoot<Guid>
{
    public int Key { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string SecurityStamp { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public List<UserToken> UserTokens { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}
