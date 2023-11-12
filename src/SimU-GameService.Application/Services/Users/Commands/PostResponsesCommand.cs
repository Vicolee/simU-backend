using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record PostResponsesCommand(Guid UserId, IEnumerable<string> Responses) : IRequest<Unit>;