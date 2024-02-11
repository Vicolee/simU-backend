using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class AddWorldHandler : IRequestHandler<AddWorldCommand, World>
{
    private readonly IUserRepository _userRepository;
    private readonly IWorldRepository _worldRepository;

    public AddWorldHandler(IUserRepository userRepository, IWorldRepository worldRepository)
    {
        _userRepository = userRepository;
        _worldRepository = worldRepository;
    }

    public async Task<World> Handle(AddWorldCommand request, CancellationToken cancellationToken)
    {
        Guid worldId = await _worldRepository.MatchWorldCodeToWorldId(request.WorldCode) ?? throw new Exception($"World not found with join code: {request.WorldCode}");
        bool success = await _userRepository.AddWorld(request.UserId, worldId, request.IsOwner);
        if (!success)
        {
            throw new Exception("Failed to add world to user's list of worlds");
        }
        World world = await _worldRepository.GetWorld(worldId) ?? throw new Exception($"World not found with id: {worldId}");
        if (world == null)
        {
            throw new Exception($"Added world to user's list but failed to grab the world with id: {worldId} from database");
        }
        return world;
    }
}