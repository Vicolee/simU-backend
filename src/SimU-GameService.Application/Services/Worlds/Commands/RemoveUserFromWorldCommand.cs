using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record RemoveUserFromWorldCommand(Guid WorldId, Guid UserId, Guid OwnerId) : IRequest<Unit>;