using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Agents.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Handlers;

public class GetAgentSummaryHandler : IRequestHandler<GetAgentSummaryQuery, string?>
{
    private readonly IAgentRepository _agentRepository;

    public GetAgentSummaryHandler(IAgentRepository agentRepository) => _agentRepository = agentRepository;

    public async Task<string?> Handle(GetAgentSummaryQuery request, CancellationToken cancellationToken)
        => await _agentRepository.GetSummary(request.AgentId);
}