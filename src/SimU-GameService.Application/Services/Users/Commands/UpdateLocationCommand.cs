using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record UpdateLocationCommand(Guid UserId, int XCoord, int YCoord) : IRequest<Unit>;