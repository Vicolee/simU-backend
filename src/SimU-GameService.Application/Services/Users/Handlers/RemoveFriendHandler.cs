using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class RemoveFriendHandler : IRequestHandler<RemoveFriendCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public RemoveFriendHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.RemoveFriend(request.UserId, request.FriendId);
        return Unit.Value;
    }
}