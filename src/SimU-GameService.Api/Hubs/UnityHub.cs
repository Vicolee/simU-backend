using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Application.Services.Users.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Hubs;

/// <summary>
/// This class defines the methods on the server that can be invoked by the SignalR Unity client.
/// </summary>
public class UnityHub : Hub<IUnityClient>,  IUnityHub
{
    private readonly IMediator _mediator;
    private static readonly ConcurrentDictionary<Guid, string> _connectionMap = new();

    public UnityHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task OnConnectedAsync()
    {
        // create mapping between user ID and connection ID
        var userIdStr = (string?) Context.GetHttpContext()?.Request.Query["userId"];
        var userId = userIdStr is null ? throw new ArgumentNullException(nameof(userIdStr)) : Guid.Parse(userIdStr);

        _connectionMap[userId] = Context.ConnectionId;
        await base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // remove mapping between user ID and connection ID
        _connectionMap.TryRemove(
            _connectionMap.FirstOrDefault(x => x.Value == Context.ConnectionId));

        await base.OnDisconnectedAsync(exception);
    }

    private Guid? GetUserIdFromConnectionMap() => _connectionMap.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

    /// <inheritdoc/>
    public async Task AddUserToGroup(Guid groupId, Guid userId)
    {
        var requesterId = GetUserIdFromConnectionMap() ??
            throw new NotFoundException($"User ID mapping to connection ID {Context.ConnectionId}");

        await _mediator.Send(new AddUserToGroupCommand(groupId, requesterId, userId));

        // send notification to user
        if (!_connectionMap.ContainsKey(userId))
        {
            throw new NotFoundException($"Connection ID for user", userId);
        }
        await Clients.Client(_connectionMap[userId])
            .ReceiveMessage(nameof(UnityHub), $"You have been added to a group with ID {groupId}");
    }

    /// <inheritdoc/>
    public async Task RequestToJoinGroup(Guid groupId)
    {
        // get user ID from connection map
        var userId = GetUserIdFromConnectionMap() ??
            throw new NotFoundException($"User ID mapping to connection ID {Context.ConnectionId}");
        
        // check if group exists
        // get owner ID from group
        var ownerId = await _mediator.Send(
            new RequestToJoinGroupCommand(groupId));

        // send add to group request to owner
        if (!_connectionMap.ContainsKey(ownerId))
        {
            throw new NotFoundException($"Connection ID for group admin", ownerId);
        }

        await Clients.Client(_connectionMap[ownerId]).AddToGroupRequest(groupId, userId);
    }

    /// <inheritdoc/>
    public async Task RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        // get ID of request sender from connection map
        var requesterId = GetUserIdFromConnectionMap() ??
            throw new NotFoundException($"User ID mapping to connection ID {Context.ConnectionId}");

        await _mediator.Send(new RemoveUserFromGroupCommand(groupId, requesterId, userId));

        // send notification to user
        if (!_connectionMap.ContainsKey(userId))
        {
            throw new NotFoundException($"Connection ID for user", userId);
        }
        await Clients.Client(_connectionMap[userId])
            .ReceiveMessage(nameof(UnityHub), $"You have been removed from a group with ID {groupId}");
    }

    /// <inheritdoc/>
    public async Task RespondToFriendRequest(Guid userId, bool accepted)
    {
       var responderId = GetUserIdFromConnectionMap() ??
            throw new NotFoundException($"User ID mapping to connection ID {Context.ConnectionId}");

        // if friend request is not accepted, we only need to notify the requester
        if (accepted)
        {
            await _mediator.Send(new RespondToFriendRequestCommand(responderId, userId));
        }

        // notify requester of response
        if (!_connectionMap.ContainsKey(userId))
        {
            throw new NotFoundException($"Connection ID for user", userId);
        }
        await Clients.Client(_connectionMap[userId])
            .ReceiveMessage(nameof(UnityHub), $"Your friend request to user with ID {responderId} has been {(accepted ? "accepted" : "rejected")}.");
    }

    /// <inheritdoc/>
    public async Task SendFriendRequest(Guid userId)
    {
        var requesterId = GetUserIdFromConnectionMap() ??
            throw new NotFoundException($"User ID mapping to connection ID {Context.ConnectionId}");

        await _mediator.Send(new SendFriendRequestCommand(requesterId, userId));

        // notify requestee of friend request
        if (!_connectionMap.ContainsKey(userId))
        {
            throw new NotFoundException($"Connection ID for user", userId);
        }
        await Clients.Client(_connectionMap[userId])
            .ReceiveMessage(nameof(UnityHub), $"You have received a friend request from {requesterId}");
    }

    /// <inheritdoc/>
    public Task SendMessage(Guid receiverId, string message)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task UpdateLocation(Location location)
    {
        throw new NotImplementedException();
    }
}