using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.DomainEvents.Events;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Hubs.Abstractions;
using SimU_GameService.Api.OnlineStatus;

namespace SimU_GameService.Api.DomainEvents.Handlers;

public class UserRemovedFromWorldEventHandler : INotificationHandler<UserRemovedFromWorldEvent>
{
    private readonly IHubContext<UnityHub, IUnityClient> _hubContext;
    private readonly IMediator _mediator;
    private readonly ConnectionService _connectionService;

    public UserRemovedFromWorldEventHandler(
        IHubContext<UnityHub, IUnityClient> hubContext,
        IMediator mediator,
        ConnectionService connectionService)
    {
        _hubContext = hubContext;
        _mediator = mediator;
        _connectionService = connectionService;
    }

    public async Task Handle(UserRemovedFromWorldEvent notification, CancellationToken cancellationToken)
        => await _hubContext.Clients.All.OnUserRemovedFromWorldHandler(notification.UserId);
}