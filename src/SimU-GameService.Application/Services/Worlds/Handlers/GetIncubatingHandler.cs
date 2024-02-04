using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetIncubatingHandler : IRequestHandler<GetIncubatingQuery, IEnumerable<Agent?>?>
{
    private readonly IWorldRepository _worldRepository;

    public GetIncubatingHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<IEnumerable<Agent?>?> Handle(GetIncubatingQuery request, CancellationToken cancellationToken)
    {
        var agents = await _worldRepository.GetWorldAgents(request.WorldId);
        var incubatingAgents = new List<Agent?>();
        incubatingAgents.Add(agents?.Where(a => a.isHatched == false));
    }
}