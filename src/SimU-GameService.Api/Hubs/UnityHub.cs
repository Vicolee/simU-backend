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

namespace SimU_GameService.Api.Hubs;

/// <summary>
/// This class defines the methods on the server that can be invoked by the SignalR Unity client.
/// </summary>
[SignalRHub]
public class UnityHub : Hub<IUnityClient>,  IUnityHub
{
    private readonly IMediator _mediator;
    private static readonly ConcurrentDictionary<string, string> _identityMap = new();

    public UnityHub(IMediator mediator) => _mediator = mediator;

    /// <inheritdoc/>
    public override async Task OnConnectedAsync()
    {
        // create mapping between identity ID and connection ID
        var identityId = Context.User?.FindFirst("sub")?.Value 
            ?? throw new UnauthorizedAccessException("User not authenticated");
        _identityMap[identityId] = Context.ConnectionId;

        await base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // remove mapping between identity ID and connection ID
        _identityMap.TryRemove(
            _identityMap.FirstOrDefault(x => x.Value == Context.ConnectionId));

        await base.OnDisconnectedAsync(exception);
    }

    private string GetIdentityIdFromConnectionMap()
        => _identityMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

    private async Task<Guid> GetUserIdFromIdentityId(string identityId)
        => await _mediator.Send(new GetUserIdFromIdentityIdQuery(identityId));
    
    private async Task<string> GetIdentityIdFromUserId(Guid userId)
        => await _mediator.Send(new GetIdentityIdFromUserIdQuery(userId));

    private async Task NotifyUser(Guid userId, string message)
    {
        var identityId = await GetIdentityIdFromUserId(userId);
        if (!_identityMap.TryGetValue(identityId, out string? connectionId))
        {
            // TODO: maybe implement a notification system for offline users
            throw new NotFoundException($"Connection ID for user", userId);
        }
        await Clients.Client(connectionId)
            .ReceiveMessage(nameof(UnityHub), message);
    }

    /// <inheritdoc/>
    public async Task AddUserToGroup(Guid groupId, Guid userId)
    {
        var requesterIdentifier = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        var requesterId = await GetUserIdFromIdentityId(requesterIdentifier);

        await _mediator.Send(new AddUserToGroupCommand(groupId, userId, requesterId));
        await NotifyUser(userId, $"You have been added to a group with ID {groupId}");
    }

    /// <inheritdoc/>
    public async Task RequestToJoinGroup(Guid groupId)
    { 
        var ownerId = await _mediator.Send(
            new GetGroupOwnerQuery(groupId));

        if (!_identityMap.TryGetValue(await GetIdentityIdFromUserId(ownerId), out string? connectionId))
        {
            throw new NotFoundException($"Connection ID for group admin", ownerId);
        }

        var userId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await Clients.Client(connectionId).AddToGroupRequest(groupId, await GetUserIdFromIdentityId(userId));
    }

    /// <inheritdoc/>
    public async Task RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        var requesterIdentityId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await _mediator.Send(
            new RemoveUserFromGroupCommand(groupId, await GetUserIdFromIdentityId(requesterIdentityId), userId));
        await NotifyUser(userId, $"You have been removed from a group with ID {groupId}");
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task SendFriendRequest(Guid userId)
    {
        var requesterId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await _mediator.Send(new SendFriendRequestCommand(await GetUserIdFromIdentityId(requesterId), userId));
        await NotifyUser(userId, $"You have received a friend request from user with ID {requesterId}");
    }

    /// <inheritdoc/>
    public async Task SendChat(Guid receiverId, string message)
    {
        var senderId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        var chatResponse = await _mediator.Send(
            new SendChatCommand(await GetUserIdFromIdentityId(senderId), receiverId, message));

        // notify receiver of message
        // TODO: figure out how to implement group notifications
        if (_identityMap.TryGetValue(await GetIdentityIdFromUserId(receiverId), out string? connectionId))
        {
            await Clients.Client(connectionId).ReceiveMessage(senderId.ToString(), message);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateLocation(int x_coord, int y_coord)
    {
        var senderId = GetIdentityIdFromConnectionMap() ??
            throw new NotFoundException(nameof(User), Context.ConnectionId);
        await _mediator.Send(new UpdateLocationCommand(await GetUserIdFromIdentityId(senderId), x_coord, y_coord));

        // broadcast location update to all users
        await Clients.All.ReceiveMessage(nameof(UnityHub), $"User {senderId} has moved to ({x_coord}, {y_coord})");
    }
}