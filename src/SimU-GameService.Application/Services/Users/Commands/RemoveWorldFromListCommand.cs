
using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record RemoveWorldFromListCommand(Guid UserId, Guid WorldId) : IRequest<Unit>;