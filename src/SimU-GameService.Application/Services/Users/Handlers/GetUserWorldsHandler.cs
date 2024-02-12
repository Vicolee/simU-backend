using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetUserWorldsHandler : IRequestHandler<GetUserWorldsQuery, IEnumerable<World>>
{
    private readonly IUserRepository _userRepository;
    private readonly IWorldRepository _worldRepository;

    public GetUserWorldsHandler(IUserRepository userRepository, IWorldRepository worldRepository)
    {
        _userRepository = userRepository;
        _worldRepository = worldRepository;
    }

    public async Task<IEnumerable<World>> Handle(GetUserWorldsQuery request, CancellationToken cancellationToken)
    {
        var worldIds = await _userRepository.GetUserWorlds(request.Id);
        return worldIds.Select(id => _worldRepository.GetWorld(id)?.Result
            ?? throw new NotFoundException(nameof(World), id));
    }
}