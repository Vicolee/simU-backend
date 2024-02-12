using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record AddAgentCommand(Guid WorldId, Guid AgentId) : IRequest<Unit>;