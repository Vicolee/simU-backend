
using MediatR;

namespace SimU_GameService.Application.Services.Agents.Commands;

public record PostAgentSpriteCommand(Guid AgentId, string Description) : IRequest<Dictionary<string, string>?>;