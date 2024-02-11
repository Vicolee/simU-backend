
using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record UpdateUserSpriteCommand(Guid UserId, Uri SpriteURL, Uri SpriteHeadshotURL) : IRequest<Unit>;