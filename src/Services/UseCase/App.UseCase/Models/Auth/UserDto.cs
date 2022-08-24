using App.Domain.Entities;
using App.UseCase.Common;

namespace App.UseCase.Models.Auth;

public class UserDto : IAutoMap<User, UserDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
}
