using MediatR;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetUserIdFromIdentityIdQuery(string IdentityId) : IRequest<Guid>;