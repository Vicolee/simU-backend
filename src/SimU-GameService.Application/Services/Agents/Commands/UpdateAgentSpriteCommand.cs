
using MediatR;

namespace SimU_GameService.Application.Services.Agents.Commands;

public record UpdateAgentSpriteCommand(Guid AgentId, Uri SpriteURL, Uri SpriteHeadshotURL) : IRequest<Unit>;