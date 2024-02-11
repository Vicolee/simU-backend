using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetHatchedAgentsHandler : IRequestHandler<GetHatchedAgentsQuery, IEnumerable<Agent>>
{
    private readonly IWorldRepository _worldRepository;

    public GetHatchedAgentsHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<IEnumerable<Agent>> Handle(GetHatchedAgentsQuery request, CancellationToken cancellationToken)
    {
        var agents = await _worldRepository.GetWorldAgents(request.WorldId);
        return (from agent in agents
                where agent.IsHatched
                select agent).ToList();
    }
}