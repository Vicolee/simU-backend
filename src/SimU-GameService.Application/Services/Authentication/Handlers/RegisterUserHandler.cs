using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Authentication.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Authentication.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid?>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public RegisterUserHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    public async Task<Guid?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserByEmail(request.Email) != null)
        {
            return null;
        }

        string identityId = await _authenticationService.RegisterUser(
            request.Email, request.Password, cancellationToken);

        var user = new User(
            identityId,
            request.FirstName,
            request.LastName,
            request.Email,
            false,
            string.Empty
        );

        await _userRepository.AddUser(user);
        return user.UserId;
    }
}