using MediatR;

namespace SimU_GameService.Application.Services.Groups.Commands;

public record RemoveUserFromGroupCommand(Guid GroupId, Guid RequesterId, Guid UserId) : IRequest<Unit>;