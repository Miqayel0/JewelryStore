using App.UseCase.Interfaces.Commands;
using App.UseCase.Interfaces.Queries;
using App.UseCase.Interfaces.Repositories;
using App.UseCase.Models.Auth;
using AutoMapper;

namespace App.UseCase.Query.V1;

public class UserInfoQuery : IUserInfoQuery
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUserSessionCommand _userSessionCommand;

    public UserInfoQuery(
        IMapper mapper,
        IUserRepository userRepository,
        IUserSessionCommand userSessionCommand)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _userSessionCommand = userSessionCommand;
    }

    public async Task<UserDto> GetUserDetailsAsync()
    {
        var userId = _userSessionCommand.GetId();
        var user = await _userRepository.FindByIdAsync(userId);
        return _mapper.Map<UserDto>(user);
    }
}
