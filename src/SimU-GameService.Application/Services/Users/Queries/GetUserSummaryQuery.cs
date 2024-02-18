using MediatR;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetUserSummaryQuery(Guid UserId) : IRequest<string?>;