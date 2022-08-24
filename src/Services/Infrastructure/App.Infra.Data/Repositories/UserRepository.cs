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
        return await Filter(x => x.Username == username).FirstOrDefaultAsync();
    }

    public async Task<List<UserToken>> FindUserTokensByIdAsync(Guid id)
    {
        return await Filter<UserToken>(x => x.UserId == id).ToListAsync();
    }

    public async Task<List<UserToken>> FindUserTokenByRefreshTokenSourceAsync(string refreshTokenSource)
    {
        return await Filter<UserToken>(x => x.RefreshTokenHash == refreshTokenSource).ToListAsync();
    }

    public async Task<UserToken> FindUserTokenAsync(string refreshToken)
    {
        var refreshTokenHash = refreshToken.Sha256();
        return await Filter<UserToken>(x => x.RefreshTokenHash == refreshTokenHash).FirstOrDefaultAsync();
    }

    public Task<User> FindUserAndTokensAsync(Guid userId)
    {
        return Filter(x => x.Id == userId)
            .Include(x => x.UserTokens)
            .FirstOrDefaultAsync();
    }
}
