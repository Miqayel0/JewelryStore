using App.Domain.Interfaces;

namespace App.Domain.Entities;

public class UserToken : IEntity<int>
{
    public int Id { get; set; }
    public string AccessTokenHash { get; set; }
    public string RefreshTokenHash { get; set; }
    public string RefreshTokenHashSource { get; set; }
    public DateTime AccessTokenExpireDate { get; set; }
    public DateTime RefreshTokenExpireDate { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}
