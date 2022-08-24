using App.Domain.Entities;
using App.Domain.Interfaces;

namespace App.UseCase.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User> FindByUsernameAsync(string username);
    Task<List<UserToken>> FindUserTokensByIdAsync(Guid id);
    Task<List<UserToken>> FindUserTokenByRefreshTokenSourceAsync(string refreshTokenSource);
    Task<UserToken> FindUserTokenAsync(string refreshToken);
    Task<User> FindUserAndTokensAsync(Guid userId);
}
