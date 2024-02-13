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

    /// <summary>
    /// Returns the AI generated summary for an agent based off the
    /// information users provided about it during its incubation process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string?> Handle(GetAgentSummaryQuery request, CancellationToken cancellationToken) {
        return await _agentRepository.GetSummary(request.AgentId);
    }
}