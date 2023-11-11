using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record RespondToFriendRequestCommand(Guid RequesterId, Guid RequesteeId) : IRequest<Unit>;