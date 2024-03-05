using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.DomainEvents.Events;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Hubs.Abstractions;

namespace SimU_GameService.Api.DomainEvents.Handlers;

public class UserLoggedInEventHandler : INotificationHandler<UserLoggedInEvent>
{
    private readonly IHubContext<UnityHub, IUnityClient> _hubContext;

    public UserLoggedInEventHandler(
        IHubContext<UnityHub, IUnityClient> hubContext) => _hubContext = hubContext;

    public async Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
        => await _hubContext.Clients.All.OnUserLoggedInHandler(notification.UserId);
}