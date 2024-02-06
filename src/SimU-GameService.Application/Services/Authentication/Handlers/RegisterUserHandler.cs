using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Authentication.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Authentication.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public RegisterUserHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserByEmail(request.Email) != null)
        {
            throw new BadRequestException("User with given email already exists.");
        }

        string identityId = await _authenticationService.RegisterUser(
            request.Email, request.Password, cancellationToken);

        var user = new User(
            identityId,
            request.Username,
            request.Email,
            false);

        await _userRepository.AddUser(user);
        return Unit.Value;
    }
}