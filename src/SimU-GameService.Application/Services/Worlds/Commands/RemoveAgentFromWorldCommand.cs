using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record RemoveAgentFromWorldCommand(Guid WorldId, Guid AgentId, Guid DeleterId) : IRequest<Unit>;