using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record RemoveUserCommand(Guid Id, Guid UserId, string CreatorIdentityId) : IRequest<Unit>;