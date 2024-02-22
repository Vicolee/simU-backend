using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record UpdateUserSpriteCommand(Guid UserId, string Description, bool IsURL) : IRequest<Unit>;