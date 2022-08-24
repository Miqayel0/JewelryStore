using App.Domain.Entities;
using App.UseCase.Models.Auth;

namespace App.UseCase.Interfaces.Commands;

public interface ITokenStoreCommand
{
    Task<TokenDto> CreateJwtTokensAsync(User user, string oldRefreshToken = null);
}
