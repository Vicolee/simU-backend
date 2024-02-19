using MediatR;

namespace SimU_GameService.Application.Services.Worlds.Queries;

public record GetWorldIdFromWorldCodeQuery(string WorldCode) : IRequest<Guid>;