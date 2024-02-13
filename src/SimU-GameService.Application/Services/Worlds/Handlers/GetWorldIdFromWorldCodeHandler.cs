using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetWorldIdFromWorldCodeHandler : IRequestHandler<GetWorldIdFromWorldCodeQuery, Guid>
{
    private readonly IWorldRepository _worldRepository;

    public GetWorldIdFromWorldCodeHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Guid> Handle(GetWorldIdFromWorldCodeQuery request,
    CancellationToken cancellationToken)
    {
        Guid worldId = await _worldRepository.MatchWorldCodeToWorldId(request.WorldCode) ?? throw new Exception($"No world with join code: {request.WorldCode} exists.");
        return worldId;
    }
}