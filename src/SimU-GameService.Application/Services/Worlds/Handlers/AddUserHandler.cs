using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class AddUserHandler : IRequestHandler<AddUserCommand, World>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public AddUserHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<World> Handle(AddUserCommand request,
    CancellationToken cancellationToken)
    {
        var world = await _worldRepository.AddUser(request.WorldId, request.UserId) ?? throw new Exception($"World not found with id: {request.WorldId}");
        bool success = await _userRepository.AddUserToWorld(request.UserId, request.WorldId, false);
        if (!success)
        {
            throw new Exception("Failed to add world to user's list of worlds");
        }
        return world;
    }
}