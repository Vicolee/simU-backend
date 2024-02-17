using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
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
        var world = await _worldRepository.AddUser(request.WorldId, request.UserId)
            ?? throw new NotFoundException(nameof(World), request.WorldId);
        await _userRepository.AddUserToWorld(request.UserId, request.WorldId, false);
        return world;
    }
}