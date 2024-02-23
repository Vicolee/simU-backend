using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record UpdateUserSpriteCommand(Guid UserId, List<int> Animations) : IRequest<Unit>;