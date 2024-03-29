using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Agents.Queries;

public record GetAgentQuery(Guid AgentId) : IRequest<Agent?>;