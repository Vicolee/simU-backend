using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Commands;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class RemoveAgentFromWorldHandler : IRequestHandler<RemoveAgentFromWorldCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public RemoveAgentFromWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(RemoveAgentFromWorldCommand request, CancellationToken cancellationToken)
    {
        try {
            await _worldRepository.RemoveAgent(request.WorldId, request.AgentId, request.DeleterId);
            return Unit.Value;
        }
        catch
        {
            throw;
        }
    }
}