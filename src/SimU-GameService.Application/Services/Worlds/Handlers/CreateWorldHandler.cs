using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class CreateWorldHandler : IRequestHandler<CreateWorldCommand, Guid>
{
    private readonly IWorldRepository _worldRepository;

    public CreateWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Guid> Handle(CreateWorldCommand request, CancellationToken cancellationToken)
    {
        var world = new World(request.Name, request.Description, request.CreatorId);
        await _worldRepository.CreateWorld(world);
        return world.Id;
    }
}