using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetWorldHandler : IRequestHandler<GetWorldQuery, World?>
{
    private readonly IWorldRepository _worldRepository;

    public GetWorldHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<World?> Handle(GetWorldQuery request,
    CancellationToken cancellationToken) => await _worldRepository.GetWorld(request.WorldId);
}