using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class AddAgentToWorldHandler : IRequestHandler<AddAgentToWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public AddAgentToWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(AddAgentToWorldCommand request,
    CancellationToken cancellationToken) => await _worldRepository.AddAgentToWorld(request.WorldId, request.AgentId);
}