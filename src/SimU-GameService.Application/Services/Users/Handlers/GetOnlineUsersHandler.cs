using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Queries;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetOnlineUsersHandler : IRequestHandler<GetOnlineUsersQuery, IEnumerable<(Guid, string)>>
{
    private readonly IUserRepository _userRepository;

    public GetOnlineUsersHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<IEnumerable<(Guid, string)>> Handle(GetOnlineUsersQuery request,
        CancellationToken cancellationToken)
            => await _userRepository.GetOnlineUsers();
}