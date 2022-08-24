using App.Domain.Entities;
using App.UseCase.Models.Auth;

namespace App.UseCase.Interfaces.Commands;

public interface IUserAccountCommand
{
    Task<LoginResultDto> LoginAsync(LoginDto dto);
    Task<RegisterResultDto> RegisterAsync(RegisterDto dto);
    Task<UserToken> AuthenticateRefreshTokenAsync(string refreshToken);
    Task<TokenDto> CreateUserSessionRefreshAsync(Guid userId, string refreshToken = null);
}
