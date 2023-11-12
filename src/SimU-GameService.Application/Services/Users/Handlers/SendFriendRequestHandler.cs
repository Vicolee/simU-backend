using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class SendFriendRequest : IRequestHandler<SendFriendRequestCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public SendFriendRequest(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
    {
        // basically, we just need to check if the requester and requestee exist before we can send the request
        var requester = await _userRepository.GetUser(request.RequesterId) ?? throw new NotFoundException(nameof(User), request.RequesterId);
        var requestee = await _userRepository.GetUser(request.RequesteeId) ?? throw new NotFoundException(nameof(User), request.RequesteeId);
        return Unit.Value;
    }
}