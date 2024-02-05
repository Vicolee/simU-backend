using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetHatchedHandler : IRequestHandler<GetHatchedQuery, IEnumerable<Agent?>?>
{
    private readonly IWorldRepository _worldRepository;

    public GetHatchedHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<IEnumerable<Agent?>?> Handle(GetHatchedQuery request, CancellationToken cancellationToken)
    {
        var agents = await _worldRepository.GetWorldAgents(request.WorldId);
        var incubatingAgents = new List<Agent?>();
        if (agents != null)
        {
            foreach (var agent in agents)
            {
                if (agent != null && agent.isHatched == true)
                {
                    incubatingAgents.Add(agent);
                }
            }
        }
        return incubatingAgents;
    }
}