using App.UseCase.Models.Auth;

namespace App.UseCase.Interfaces.Queries;

public interface IUserInfoQuery
{
    Task<UserDto> GetUserDetailsAsync();
}
