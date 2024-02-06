using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Worlds.Queries;

public record GetIncubatingQuery(Guid WorldId) : IRequest<IEnumerable<Agent?>?>;