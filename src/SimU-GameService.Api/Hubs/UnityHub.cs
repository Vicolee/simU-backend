using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SimU_GameService.Api.Hubs.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Chats.Commands;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Application.Services.Groups.Queries;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Worlds.Queries;

namespace SimU_GameService.Api.Hubs;

/// <summary>
/// This class defines the methods on the server that can be invoked by the SignalR Unity client.
/// </summary>
[SignalRHub]
public class UnityHub : Hub<IUnityClient>, IUnityServer
{
    private readonly IMediator _mediator;
    private readonly Timer _timer;
    private static readonly TimeSpan _pingInterval = TimeSpan.FromMinutes(3);

    public UnityHub(IMediator mediator)
    {
        _mediator = mediator;
        _timer = new Timer(PingClient, null, TimeSpan.Zero, _pingInterval);
    }

    /// <summary>
    /// A mapping between user identity IDs and connection IDs.
    /// </summary>
    private static readonly ConcurrentDictionary<string, string> _connectionIdMap = new();

    /// <summary>
    /// A mapping between user identity IDs and the time of the user's last ping.
    /// </summary>
    private static readonly ConcurrentDictionary<string, DateTime> _lastPingMap = new();

    private string? GetIdentityIdFromConnectionMap()
        => _connectionIdMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

    private async Task<Guid> GetUserIdFromIdentityId(string identityId)
        => await _mediator.Send(new GetUserIdFromIdentityIdQuery(identityId));
    
    private async Task<string> GetIdentityIdFromUserId(Guid userId)
        => await _mediator.Send(new GetIdentityIdFromUserIdQuery(userId));

    private async Task NotifyUser(Guid userId, string message)
    {
        var identityId = await GetIdentityIdFromUserId(userId);
        if (!_connectionIdMap.TryGetValue(identityId, out string? connectionId))
        {
            // TODO: maybe implement a notification system for offline users
            throw new NotFoundException($"Connection ID for user", userId);
        }
        await Clients.Client(connectionId)
            .MessageHandler(nameof(UnityHub), message);
    }

    public override async Task OnConnectedAsync()
    {
        // create mapping between identity ID and connection ID
        var identityId = Context.User?.FindFirst("sub")?.Value 
            ?? throw new UnauthorizedAccessException("User not authenticated");
        _connectionIdMap[identityId] = Context.ConnectionId;

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // remove mapping between identity ID and connection ID
        _connectionIdMap.TryRemove(
            _connectionIdMap.FirstOrDefault(x => x.Value == Context.ConnectionId));

        await base.OnDisconnectedAsync(exception);
    }

    public async Task AddUserToGroup(Guid groupId, Guid userId)
    {
        var requesterIdentifier = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        var requesterId = await GetUserIdFromIdentityId(requesterIdentifier);

        await _mediator.Send(new AddUserToGroupCommand(groupId, userId, requesterId));
        await NotifyUser(userId, $"You have been added to a group with ID {groupId}");
    }

    public async Task RequestToJoinGroup(Guid groupId)
    {
        var ownerId = await _mediator.Send(
            new GetGroupOwnerQuery(groupId));

        if (!_connectionIdMap.TryGetValue(await GetIdentityIdFromUserId(ownerId), out string? connectionId))
        {
            throw new NotFoundException($"Connection ID for group admin", ownerId);
        }

        var userId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await Clients.Client(connectionId).JoinGroupRequestHandler(groupId, await GetUserIdFromIdentityId(userId));
    }

    public async Task RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        var requesterIdentityId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await _mediator.Send(
            new RemoveUserFromGroupCommand(groupId, await GetUserIdFromIdentityId(requesterIdentityId), userId));
        await NotifyUser(userId, $"You have been removed from a group with ID {groupId}");
    }

    public async Task RespondToFriendRequest(Guid userId, bool accepted)
    {
       var responderIdentityId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException($"User ID mapping to connection ID {Context.ConnectionId}");

        if (accepted)
        {
            await _mediator.Send(new AcceptFriendRequestCommand(
                await GetUserIdFromIdentityId(responderIdentityId), userId));
        }

        await NotifyUser(userId, 
            $"Your friend request to user with ID {responderIdentityId} has been {(accepted ? "accepted" : "rejected")}.");
    }

    public async Task SendFriendRequest(Guid userId)
    {
        var requesterId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await _mediator.Send(new SendFriendRequestCommand(await GetUserIdFromIdentityId(requesterId), userId));
        await NotifyUser(userId, $"You have received a friend request from user with ID {requesterId}");
    }

    public async Task SendChat(Guid receiverId, string message)
    {
        var senderId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        var chatResponse = await _mediator.Send(
            new SendChatCommand(await GetUserIdFromIdentityId(senderId), receiverId, message));

        // notify receiver of message
        if (_connectionIdMap.TryGetValue(await GetIdentityIdFromUserId(receiverId), out string? connectionId))
        {
            await Clients.Client(connectionId).MessageHandler(senderId.ToString(), message);
        }
    }

    public async Task UpdateLocation(Location location)
    {
        var senderId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await _mediator.Send(new UpdateLocationCommand(
            await GetUserIdFromIdentityId(senderId), location.X_coord, location.Y_coord));

        // broadcast location update to all users
        await Clients.All.MessageHandler(nameof(UnityHub),
            $"User {senderId} has moved to ({location.X_coord}, {location.Y_coord})");
    }

    public void PingServer()
    {
        var identityId = GetIdentityIdFromConnectionMap()
            ?? throw new NotFoundException(nameof(User), Context.ConnectionId);
        _lastPingMap[identityId] = DateTime.Now;
    }


    /// <summary>
    /// Sends a ping request to the client to check if the client is still logged in.
    /// Removes users who have not pinged the server since the last contact.
    /// </summary>
    /// <param name="state">Default is null</param>
    private async void PingClient(object? state)
    {
        // logout users who have not pinged the server since the last ping
        foreach (var (identityId, lastPing) in _lastPingMap)
        {
            if (DateTime.Now - lastPing > _pingInterval)
            {
                await LogoutUser(identityId);
            }
        }

        // for each logged-in user, send a ping request to the client
        foreach (var identityId in _connectionIdMap.Keys)
        {
            if (_connectionIdMap.TryGetValue(identityId, out string? connectionId))
            {
                await Clients.Client(connectionId).UserOnlineCheckHandler();
            }
        }  
    }

    /// <summary>
    /// Logs out a user from the server and notifies users in the same world that the user has logged out.
    /// </summary>
    /// <param name="identityId">The identity ID of the user to log out.</param>
    /// <returns></returns>
    private async Task LogoutUser(string identityId)
    {
        _connectionIdMap.TryRemove(identityId, out _);
        _lastPingMap.TryRemove(identityId, out _);

        var userId = await GetUserIdFromIdentityId(identityId);
        await _mediator.Send(new LogoutUserCommand(userId));

        var (worldName, worldUserIdentities) = await _mediator.Send(new GetUsersInSameWorldQuery(userId));
        foreach (var userIdentityId in worldUserIdentities)
        {
            if (_connectionIdMap.TryGetValue(userIdentityId, out string? connectionId))
            {
                await Groups.AddToGroupAsync(connectionId!, $"world_{worldName}");
            }
        }
        await Clients.Group(worldName).OnUserLoggedOutHandler(userId);
    }
}