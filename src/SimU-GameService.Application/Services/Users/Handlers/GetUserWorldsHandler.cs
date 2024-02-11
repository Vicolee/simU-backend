using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetUserWorldsHandler : IRequestHandler<GetUserWorldsQuery, IEnumerable<Guid>>
{
    private readonly IUserRepository _userRepository;

    public GetUserWorldsHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<IEnumerable<Guid>> Handle(GetUserWorldsQuery request,
        CancellationToken cancellationToken)
        {
            var worlds = await _userRepository.GetUserWorlds(request.UserId);
            return worlds.Cast<Guid>();
        }
}