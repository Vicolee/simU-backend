using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Hubs.Abstractions;

/// <summary>
/// Defines the methods on the client-side that can be invoked by the <see cref="UnityHub"/> (server).
/// </summary>
public interface IUnityClient
{
    /// <summary>
    /// Handles a message received from another client through the server.
    /// </summary>
    /// <param name="sender">The client (can also be the server) sending the message</param>
    /// <param name="message">The content of the message</param>
    /// <returns></returns>
    Task MessageHandler(string sender, string message);

    /// <summary>
    /// Handles a server request that updates the location of a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    Task UpdateLocationHandler(Guid userId, Location location);

    /// <summary>
    /// Handles a request to join a group.
    /// </summary>
    /// <param name="groupId">The ID of the group to which the join request is being made</param>
    /// <param name="userId">The ID of the user requesting to join the group</param>
    /// <returns></returns>
    Task JoinGroupRequestHandler(Guid groupId, Guid userId);

    /// <summary>
    /// Handles a server request that checks if the user is online.
    /// The server will routinely send this request to check if the client is still logged in.
    /// The client should respond by sending a <see cref="IUnityServer.PingServer"/> request.
    /// </summary>
    /// <returns></returns>
    Task UserOnlineCheckHandler();

    /// <summary>
    /// Handles a server request that notifies clients when a world's user logs into the server.
    /// </summary>
    /// <param name="userId">The ID of the newly-logged in user</param>
    /// <returns></returns>
    Task OnUserLoggedInHandler(Guid userId);

    /// <summary>
    /// Handles a server request that notifies clients when a world's user logs out of the server.
    /// </summary>
    /// <param name="userId">The ID of the newly-logged out user</param>
    /// <returns></returns>
    Task OnUserLoggedOutHandler(Guid userId);

    /// <summary>
    /// Handles a server request that notifies clients when a new agent is added to the world.
    /// </summary>
    /// <param name="agentId">The ID of the newly-added agent</param>
    /// <returns></returns>
    Task OnAgentAddedHandler(Guid agentId);

    /// <summary>
    /// Handles a server request that notifies clients when a new user joins the world.
    /// </summary>
    /// <param name="userId">The ID of the newly-added user</param>
    /// <returns></returns>
    Task OnUserAddedToWorldHandler(Guid userId);

    /// <summary>
    /// Handles a server request that notifies clients when a user leaves the world.
    /// </summary>
    /// <param name="userId">The ID of the user who left the world</param>
    /// <returns></returns>
    Task OnUserRemovedFromWorldHandler(Guid userId);
}
