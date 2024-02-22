using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetWorldIdFromWorldCodeHandler : IRequestHandler<GetWorldIdFromWorldCodeQuery, Guid>
{
    private readonly IWorldRepository _worldRepository;

    public GetWorldIdFromWorldCodeHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<Guid> Handle(GetWorldIdFromWorldCodeQuery request,
    CancellationToken cancellationToken) => await _worldRepository.GetWorldIdByWorldCode(request.WorldCode)
        ?? throw new NotFoundException("World", request.WorldCode);
}