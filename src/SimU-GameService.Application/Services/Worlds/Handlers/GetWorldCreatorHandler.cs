using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetWorldCreatorHandler : IRequestHandler<GetWorldCreatorQuery, User?>
{
    private readonly IWorldRepository _worldRepository;

    public GetWorldCreatorHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<User?> Handle(GetWorldCreatorQuery request,
    CancellationToken cancellationToken) => await _worldRepository.GetWorldCreator(request.WorldId);
}