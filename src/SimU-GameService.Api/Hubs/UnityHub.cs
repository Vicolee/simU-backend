using Microsoft.AspNetCore.SignalR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Hubs;

/// <summary>
/// This class defines the methods on the server that can be invoked by the SignalR Unity client.
/// </summary>
public class UnityHub : Hub<IUnityClient>,  IUnityHub
{
    /// <inheritdoc/>
    public override async Task OnConnectedAsync()
    {
        // TODO: Add functionality later. For now, just call the base method.
        await base.OnConnectedAsync();
    }

    /// <inheritdoc/>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // TODO: Add functionality later. For now, just call the base method.
        await base.OnDisconnectedAsync(exception);
    }

    /// <inheritdoc/>
    public Task AddUser(Guid groupId, Guid ownerId, Guid userId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RequestToJoinGroup(Guid groupId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RemoveUser(Guid groupId, Guid ownerId, Guid userId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task RespondToFriendRequest(Guid userId, bool accepted)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task SendFriendRequest(Guid userId)
    {
        throw new NotImplementedException();
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