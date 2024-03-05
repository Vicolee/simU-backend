using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.DomainEvents;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Hubs.Abstractions;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;

namespace SimU_GameService.Api.OnlineStatus;

public class OnlineStatusService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConnectionService _connectionService;
    private readonly IHubContext<UnityHub, IUnityClient> _hubContext;
    private readonly TimeSpan _pingInterval = TimeSpan.FromMinutes(1);

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
            // resolve scoped services
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            // get all users currently online and their last ping times
            var onlineUserIdentities = await mediator.Send(new GetOnlineUsersQuery(), stoppingToken);
            var lastPings = onlineUserIdentities
                .Select(identityId => (identityId, _connectionService.GetLastPingTime(identityId)))
                .Where(pair => pair.Item2 != null)!;
            
            // mark users as offline if they have not responded to the last 2 ping requests
            var loggedOutUserIdentities = new List<string>();
            foreach (var (identityId, lastPing) in lastPings)
            {
                if (DateTime.UtcNow - lastPing > (2 * _pingInterval + TimeSpan.FromSeconds(5)))
                {
                    loggedOutUserIdentities.Add(identityId);
                }
            }

            foreach (var identityId in loggedOutUserIdentities)
            {
                await MarkOffline(identityId, mediator, stoppingToken);
            }

            // ping remaining online users
            var loggedInUsers = onlineUserIdentities.Except(loggedOutUserIdentities).ToList();
            var connectionIds = loggedInUsers
                .Select(_connectionService.GetConnectionId)
                .Where(connectionId => connectionId != null)!
                .ToList();
            
            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Clients.Client(connectionId!).UserOnlineCheckHandler();
            }

            // wait for the next ping interval
            await Task.Delay(_pingInterval, stoppingToken);
        }
    }

    private async Task MarkOffline(string identityId, IMediator mediator, CancellationToken cancellationToken)
    {
        // log out user
        var userId = await mediator.Send(new GetUserIdFromIdentityIdQuery(identityId), cancellationToken);
        await mediator.Send(new LogoutUserCommand(userId), cancellationToken);  
        
        // notify users in the same world that the user has logged out
        var groupName = await DomainEventsUtils.CreateSameWorldUsersGroup(
            userId, mediator, _connectionService, _hubContext, cancellationToken);
        if (groupName is not null)
        {
            await _hubContext.Clients.Group(groupName).OnUserLoggedOutHandler(userId);
        }
    }
}