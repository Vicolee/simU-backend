using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Api.Hubs;
using SimU_GameService.Api.Hubs.Abstractions;
using SimU_GameService.Api.OnlineStatus;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Api.DomainEvents;

public static class DomainEventsUtils
{
    public static async Task<string?> CreateSameWorldUsersGroup(Guid userId, 
        IMediator mediator,
        ConnectionService connectionService,
        IHubContext<UnityHub, IUnityClient> hubContext,
        CancellationToken cancellationToken)
    {
        var (worldName, worldUserIdentities) = await mediator.Send(
                    new GetUsersInSameWorldQuery(userId), cancellationToken);

        if (worldUserIdentities?.Any() != true)
        {
            return default;
        }

        var groupName = $"world_{worldName}";
        foreach (var userIdentityId in worldUserIdentities)
        {
            var connectionId = connectionService.GetConnectionId(userIdentityId);
            if (connectionId is not null)
            {
                await hubContext.Groups.AddToGroupAsync(connectionId, groupName, cancellationToken);
            }
        }
        return groupName;
    }
}