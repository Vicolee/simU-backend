using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Agents.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class GetAgentHandler : IRequestHandler<GetAgentQuery, Agent?>
{
    private readonly IAgentRepository _agentRepository;

    public GetAgentHandler(IAgentRepository agentRepository) => _agentRepository = agentRepository;

    public async Task<Agent?> Handle(GetAgentQuery request,
        CancellationToken cancellationToken) => await _agentRepository.GetAgent(request.AgentId);
}