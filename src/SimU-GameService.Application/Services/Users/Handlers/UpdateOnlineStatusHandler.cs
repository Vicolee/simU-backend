using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateOnlineStatusHandler : IRequestHandler<UpdateOnlineStatusCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public UpdateOnlineStatusHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<Unit> Handle(UpdateOnlineStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.IsOnline)
        {
            await _userRepository.Login(request.UserId);
        }
        else
        {
            await _userRepository.Logout(request.UserId);
        }
        return Unit.Value;
    }
}
