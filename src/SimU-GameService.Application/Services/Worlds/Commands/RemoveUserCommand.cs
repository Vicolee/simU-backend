using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record RemoveUserCommand(Guid WorldId, Guid UserId) : IRequest<Unit>;