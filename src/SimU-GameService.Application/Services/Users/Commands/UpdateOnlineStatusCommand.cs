using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record UpdateOnlineStatusCommand(Guid UserId, bool IsOnline) : IRequest<Unit>;