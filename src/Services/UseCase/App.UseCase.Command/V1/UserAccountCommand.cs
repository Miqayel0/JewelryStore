using App.Domain.Entities;
using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Auth;
using AutoMapper;
using Common.Exceptions;
using Common.Extensions;

namespace App.UseCase.Command.V1;

public class UserAccountCommand : IUserAccountCommand
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly ITokenStoreCommand _tokenStoreCommand;

    public UserAccountCommand(
        IMapper mapper,
        IUserRepository userRepository,
        ITokenStoreCommand tokenStoreCommand)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _tokenStoreCommand = tokenStoreCommand;
    }

    public async Task<UserToken> AuthenticateRefreshTokenAsync(string refreshToken)
    {
        var token = await _userRepository.FindUserTokenAsync(refreshToken);
        if (token == null)
            throw new GoneException();

        if (token.RefreshTokenExpireDate < DateTimeOffset.UtcNow)
            throw new TokenExpiredException();

        return token;
    }

    public async Task<TokenDto> CreateUserSessionRefreshAsync(Guid userId, string refreshToken = null)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        return await _tokenStoreCommand.CreateJwtTokensAsync(user, refreshToken);
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.FindByUsernameAsync(dto.Username);
        if (user == null)
            throw new NotFoundException("Incorrect login credentials");
        if (!VerifyUserPassword(user, dto.Password))
            throw new NotFoundException("Incorrect login credentials!");

        var token = await _tokenStoreCommand.CreateJwtTokensAsync(user);
        return new LoginResultDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user),
        };
    }

    public async Task<RegisterResultDto> RegisterAsync(RegisterDto dto)
    {
        var securityStamp = 256.Salt();
        var user = new User
        {
            SecurityStamp = securityStamp,
            PasswordHash = dto.Password.Sha256(securityStamp),
            Username = dto.Username,
            Name = dto.Name,
        };
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = await _tokenStoreCommand.CreateJwtTokensAsync(user);
        return new RegisterResultDto
        {
            Token = token,
            User = _mapper.Map<UserDto>(user),
        };
    }

    private bool VerifyUserPassword(User user, string password)
    {
        var passwordHash = password.Sha256(user.SecurityStamp);
        return user.PasswordHash.Equals(passwordHash, StringComparison.Ordinal);
    }
}
