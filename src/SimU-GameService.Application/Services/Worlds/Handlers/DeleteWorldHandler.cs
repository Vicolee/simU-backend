using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class DeleteWorldHandler : IRequestHandler<DeleteWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public DeleteWorldHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
    {
        var ownerId = await _userRepository.GetUserIdFromIdentityId(request.CreatorIdentityId);
        await _worldRepository.DeleteWorld(request.WorldId, ownerId);
        return Unit.Value;
    }
}