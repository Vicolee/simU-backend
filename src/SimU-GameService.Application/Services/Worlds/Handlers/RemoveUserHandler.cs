using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class RemoveUserHandler : IRequestHandler<RemoveUserCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public RemoveUserHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        await _worldRepository.RemoveUser(request.Id, request.OwnerId, request.UserId);
        await _userRepository.RemoveUserFromWorld(request.UserId, request.Id);
        return Unit.Value;
    }
}