using App.Domain.Entities;
using App.Infra.Data.Common;
using App.UseCase.Interfaces.Repositories;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Infra.Data.Repositories;

public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User> FindByUsernameAsync(string username)
    {
        return await Filter(e => e.Username == username).FirstOrDefaultAsync();
    }

    public async Task<List<UserToken>> FindUserTokensByIdAsync(Guid id)
    {
        return await Filter<UserToken>(e => e.UserId == id).ToListAsync();
    }

    public async Task<List<UserToken>> FindUserTokenByRefreshTokenSourceAsync(string refreshTokenSource)
    {
        return await Filter<UserToken>(e => e.RefreshTokenHash == refreshTokenSource).ToListAsync();
    }

    public async Task<UserToken> FindUserTokenAsync(string refreshToken)
    {
        var refreshTokenHash = refreshToken.Sha256();
        return await Filter<UserToken>(e => e.RefreshTokenHash == refreshTokenHash).FirstOrDefaultAsync();
    }

    public Task<User> FindUserAndTokensAsync(Guid userId)
    {
        return Filter(e => e.Id == userId)
            .Include(e => e.UserTokens)
            .FirstOrDefaultAsync();
    }
}
