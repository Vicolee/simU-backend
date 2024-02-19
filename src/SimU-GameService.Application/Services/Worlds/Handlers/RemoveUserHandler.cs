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
        var creatorId = await _userRepository.GetUserFromIdentityId(request.CreatorIdentityId);
        await _userRepository.RemoveUserFromWorld(request.UserId, request.Id);
        await _worldRepository.RemoveUser(request.Id, creatorId, request.UserId);
        return Unit.Value;
    }
}