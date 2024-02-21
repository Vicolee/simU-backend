using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record AcceptFriendRequestCommand(Guid RequesterId, Guid RequesteeId) : IRequest<Unit>;