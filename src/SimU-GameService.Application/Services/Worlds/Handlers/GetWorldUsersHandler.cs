using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetWorldUsersHandler : IRequestHandler<GetWorldUsersQuery, IEnumerable<User>>
{
    private readonly IWorldRepository _worldRepository;

    public GetWorldUsersHandler(IWorldRepository worldRepository) => _worldRepository = worldRepository;

    public async Task<IEnumerable<User>> Handle(GetWorldUsersQuery request,
    CancellationToken cancellationToken) => await _worldRepository.GetWorldUsers(request.WorldId);
}