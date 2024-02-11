using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record UpdateUserSummaryCommand(Guid UserId, string Summary) : IRequest<Unit>;