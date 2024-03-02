using MediatR;

namespace SimU_GameService.Application.Services.Responses.Queries;

public record CheckIfUserQuery(Guid TargetId) : IRequest<bool>;