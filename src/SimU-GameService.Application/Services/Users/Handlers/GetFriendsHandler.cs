using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetFriendsHandler : IRequestHandler<GetFriendsQuery, IEnumerable<Friend>>
{
    private readonly IUserRepository _userRepository;

    public GetFriendsHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<IEnumerable<Friend>> Handle(
        GetFriendsQuery request,
        CancellationToken cancellationToken) => await _userRepository.GetFriends(request.UserId);
}