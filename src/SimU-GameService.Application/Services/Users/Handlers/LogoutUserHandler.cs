using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public LogoutUserHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.Logout(request.UserId);
        return Unit.Value;
    }
}