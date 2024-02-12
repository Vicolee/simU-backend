using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetCreatorHandler : IRequestHandler<GetCreatorQuery, User?>
{
    private readonly IWorldRepository _worldRepository;

    public GetCreatorHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<User?> Handle(GetCreatorQuery request,
    CancellationToken cancellationToken) => await _worldRepository.GetCreator(request.WorldId);
}