using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Worlds.Queries;

public record GetIncubatingAgentsQuery(Guid WorldId) : IRequest<IEnumerable<Agent>>;