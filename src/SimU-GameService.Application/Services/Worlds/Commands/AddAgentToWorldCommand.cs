using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record AddAgentToWorldCommand(Guid WorldId, Guid AgentId) : IRequest<Unit>;