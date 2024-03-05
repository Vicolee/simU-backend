using MediatR;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetOnlineUsersQuery : IRequest<IEnumerable<string>>;