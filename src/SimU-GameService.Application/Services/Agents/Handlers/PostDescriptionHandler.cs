using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class PostDescriptionHandler : IRequestHandler<PostDescriptionCommand, Unit>
{
    private readonly IAgentRepository _agentRepository;

    public PostDescriptionHandler(IAgentRepository agentRepository)
        => _agentRepository = agentRepository;

    public async Task<Unit> Handle(PostDescriptionCommand request, CancellationToken cancellationToken)
    {
        await _agentRepository.UpdateDescription(request.AgentId, request.Description);
        return Unit.Value;
    }
}