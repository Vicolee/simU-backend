using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record RemoveFriendCommand(Guid UserId, Guid FriendId) : IRequest<Unit>;