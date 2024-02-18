using MediatR;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Authentication.Commands;

namespace SimU_GameService.Application.Services.Authentication.Handlers;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Tuple<Guid, string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public LoginUserHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    public async Task<Tuple<Guid, string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email)
            ?? throw new BadRequestException("Invalid login credentials.");
        var authToken = await _authenticationService.LoginUser(request.Email, request.Password);
        await _userRepository.MarkOnline(user.Id);
        return new Tuple<Guid, string>(user.Id, authToken);
    }
}