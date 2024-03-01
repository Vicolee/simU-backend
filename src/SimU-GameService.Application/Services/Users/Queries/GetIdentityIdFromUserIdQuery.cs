using MediatR;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetIdentityIdFromUserIdQuery(Guid UserId) : IRequest<string?>;