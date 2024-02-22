using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Worlds.Queries;

public record GetUsersInSameWorldQuery(Guid UserId) : IRequest<(string, IEnumerable<string>)>;