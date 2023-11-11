using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record SendFriendRequestCommand(Guid RequesterId, Guid RequesteeId) : IRequest<Unit>;