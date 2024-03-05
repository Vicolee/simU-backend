using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.DomainEvents.Events;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Hubs.Abstractions;

namespace SimU_GameService.Api.DomainEvents.Handlers;

public class UserLoggedOutEventHandler : INotificationHandler<UserLoggedOutEvent>
{
    private readonly IHubContext<UnityHub, IUnityClient> _hubContext;

    public UserLoggedOutEventHandler(
        IHubContext<UnityHub, IUnityClient> hubContext) => _hubContext = hubContext;

    public async Task Handle(UserLoggedOutEvent notification, CancellationToken cancellationToken)
        => await _hubContext.Clients.All.OnUserLoggedOutHandler(notification.UserId);
}