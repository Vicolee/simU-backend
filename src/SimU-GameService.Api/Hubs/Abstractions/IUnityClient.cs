using SimU_GameService.Contracts.Responses;
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
    /// <param name="message">The message sent by the client</param>
    /// <returns></returns>
    Task ChatHandler(ChatResponse? message);

    /// <summary>
    /// Handles a message that notifies the client when a user moves to a new location.
    /// </summary>
    /// <param name="userId">The ID of the user who moved</param>
    /// <param name="location">The new location of the user</param>
    /// <returns></returns>
    Task UpdateLocationHandler(Guid userId, Location location);

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