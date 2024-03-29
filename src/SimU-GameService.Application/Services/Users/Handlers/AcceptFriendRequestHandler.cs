using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class AcceptFriendRequestHandler : IRequestHandler<AcceptFriendRequestCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public AcceptFriendRequestHandler(IUserRepository userRepository) => _userRepository = userRepository;
    
    public async Task<Unit> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
    {
        // add the requester to the requestee's friends list and vice versa
        await _userRepository.AddFriend(request.RequesterId, request.RequesteeId);
        return Unit.Value;
    }
}