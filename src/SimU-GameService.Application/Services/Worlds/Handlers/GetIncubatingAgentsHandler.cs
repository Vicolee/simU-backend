using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;
using System.Linq;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetIncubatingAgentsHandler : IRequestHandler<GetIncubatingAgentsQuery, IEnumerable<Agent>>
{
    private readonly IWorldRepository _worldRepository;

    public GetIncubatingAgentsHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<IEnumerable<Agent>> Handle(GetIncubatingAgentsQuery request, CancellationToken cancellationToken)
    {
        var agents = await _worldRepository.GetWorldAgents(request.WorldId);
        return (from agent in agents
                where !agent.IsHatched
                select agent).ToList();
    }
}