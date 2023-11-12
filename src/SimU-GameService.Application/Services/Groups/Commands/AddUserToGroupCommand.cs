using MediatR;

namespace SimU_GameService.Application.Services.Groups.Commands;

public record AddUserToGroupCommand(Guid GroupId, Guid RequesterId, Guid UserId) : IRequest<Unit>;