using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Hubs.Abstractions;

/// <summary>
/// This interface specifies the methods on the server that can be invoked by the client.
/// </summary>
public interface IUnityServer
{
    /// <summary>
    /// Updates the location of the user to the given location.
    /// </summary>
    /// <param name="location">The user's new coordinates</param>
    /// <returns></returns>
    Task UpdateLocation(Location location);

    /// <summary>
    /// Sends a <paramref name="message"/> to the user with the given <paramref name="receiverId"/>
    /// </summary>
    /// <param name="receiverId">The ID of the message target</param>
    /// <param name="message">The actual contents of the message</param>
    /// <returns></returns>
    Task SendChat(Guid receiverId, string message);

    /// <summary>
    /// Notifies the server that the user is online. We use this to keep track of active users. 
    /// </summary>
    /// <returns></returns>
    void PingServer();
}