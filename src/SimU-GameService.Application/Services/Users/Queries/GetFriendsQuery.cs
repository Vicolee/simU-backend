using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetFriendsQuery(Guid UserId) : IRequest<IEnumerable<Friend>>;