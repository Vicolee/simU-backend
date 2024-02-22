using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Worlds.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Worlds.Handlers;

public class GetUsersInSameWorldHandler : IRequestHandler<GetUsersInSameWorldQuery, (string, IEnumerable<string>)>
{
    private readonly IWorldRepository _worldRepository;
    private readonly IUserRepository _userRepository;

    public GetUsersInSameWorldHandler(IWorldRepository worldRepository, IUserRepository userRepository)
    {
        _worldRepository = worldRepository;
        _userRepository = userRepository;
    }

    public async Task<(string, IEnumerable<string>)> Handle(GetUsersInSameWorldQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(request.UserId)
            ?? throw new NotFoundException(nameof(User), request.UserId);
        
        if (user.ActiveWorldId == default)
        {
            return (default!, Array.Empty<string>());
        }

        var worldUserIds = (await _worldRepository.GetWorldUsers(user.ActiveWorldId)).Select(u => u.IdentityId);
        var worldName = (await _worldRepository.GetWorld(user.ActiveWorldId))?.Name
            ?? throw new NotFoundException(nameof(World), user.ActiveWorldId);
        
        return (worldName, worldUserIds);
    }
}