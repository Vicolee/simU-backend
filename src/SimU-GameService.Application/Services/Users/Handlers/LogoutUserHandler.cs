using MediatR;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    public Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}