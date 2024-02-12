using MediatR;

namespace SimU_GameService.Application.Services.Agents.Queries;

public record GetAgentSummaryQuery(Guid AgentId) : IRequest<string?>;