using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record RemoveAgentCommand(Guid Id, Guid AgentId, Guid CreatorId) : IRequest<Unit>;
