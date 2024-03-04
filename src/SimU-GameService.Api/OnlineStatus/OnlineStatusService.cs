using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Hubs.Abstractions;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Api.OnlineStatus;

public class OnlineStatusService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConnectionService _connectionService;
    private readonly IHubContext<UnityHub, IUnityClient> _hubContext;
    private readonly TimeSpan _pingInterval = TimeSpan.FromMinutes(5);

    public OnlineStatusService(IServiceProvider serviceProvider,
        ConnectionService connectionService, IHubContext<UnityHub, IUnityClient> hubContext)
    {
        _hubContext = hubContext;
        _serviceProvider = serviceProvider;
        _connectionService = connectionService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var lastPingMap = _connectionService.GetAllLastPingTimes();
            foreach (var (identityId, lastPing) in lastPingMap)
            {
                // if the user has not responded to the last 3 ping requests, log them out
                if (DateTime.UtcNow - lastPing > (3 * _pingInterval))
                {
                    await LogoutUser(identityId);
                }
            }

            // for each logged-in user, send a ping request to the client
            var connectionIds = _connectionService.GetAllConnectionIds();
            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Clients.Client(connectionId).UserOnlineCheckHandler();
            }
            await Task.Delay(_pingInterval, stoppingToken);
        }
    }

    /// <summary>
    /// Logs out a user from the server and notifies users in the same world that the user has logged out.
    /// </summary>
    /// <param name="identityId">The identity ID of the user to log out.</param>
    /// <returns></returns>
    public async Task LogoutUser(string identityId)
    {
        _connectionService.RemoveConnectionByIdentityId(identityId);
        _connectionService.RemoveLastPingTime(identityId);

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var userId = await mediator.Send(new GetUserIdFromIdentityIdQuery(identityId));
        await mediator.Send(new LogoutUserCommand(userId));

        var (worldName, worldUserIdentities) = await mediator.Send(new GetUsersInSameWorldQuery(userId));
        var groupName = $"world_{worldName}";
        foreach (var userIdentityId in worldUserIdentities)
        {
            var connectionId = _connectionService.GetConnectionId(userIdentityId);
            if (connectionId is not null)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
            }
        }
        await _hubContext.Clients.Group(groupName).OnUserLoggedOutHandler(userId);
    }
}