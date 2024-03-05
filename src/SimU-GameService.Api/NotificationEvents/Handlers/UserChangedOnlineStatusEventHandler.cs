using MediatR;
using SimU_GameService.Api.DomainEvents.Events;
using SimU_GameService.Application.Abstractions.Repositories;

namespace SimU_GameService.Api.NotificationEvents.Handlers;

public class UserChangedOnlineStatusEventHandler : INotificationHandler<UserChangedOnlineStatusEvent>
{
    private readonly IUserRepository _userRepository;

    public UserChangedOnlineStatusEventHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task Handle(UserChangedOnlineStatusEvent notification, CancellationToken cancellationToken)
    {
        if (notification.IsOnline)
        {
            await _userRepository.Login(notification.UserId);
        }
        else
        {
            await _userRepository.Logout(notification.UserId);
        }
    }
}