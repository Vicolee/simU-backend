using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Worlds.Commands;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class AddAgentHandler : IRequestHandler<AddAgentCommand, Unit>
{
    private readonly IWorldRepository _worldRepository;

    public AddAgentHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Unit> Handle(AddAgentCommand request,
    CancellationToken cancellationToken)
    {
        await _worldRepository.AddAgent(request.WorldId, request.AgentId);
        return Unit.Value;
    }
}