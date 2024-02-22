
using MediatR;

namespace SimU_GameService.Application.Services.Agents.Commands;

public record PostVisualDescriptionCommand(Guid AgentId, string Description) : IRequest<(Uri SpriteURL, Uri SpriteHeadshotURL)>;