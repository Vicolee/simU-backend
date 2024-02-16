using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

// TODO: broadcast location update to only the users in the same world
public record UpdateLocationCommand(Guid UserId, int XCoord, int YCoord) : IRequest<Unit>;