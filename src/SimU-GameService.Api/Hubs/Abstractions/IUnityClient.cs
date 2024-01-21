
namespace SimU_GameService.Api.Hubs;

/// <summary>
/// Defines the methods on the client-side that can be invoked by the <see cref="UnityHub"/> (server).
/// </summary>
public interface IUnityClient
{
    // TODO: Add client methods to be invoked by the server here.

    /// <summary>
    /// Handles a message received from another client through the server.
    /// </summary>
    /// <param name="sender">The client (can also be the server) sending the message</param>
    /// <param name="message">The content of the message</param>
    /// <returns></returns>
    Task ReceiveMessage(string sender, string message);

    /// <summary>
    /// Handles a request to join a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task AddToGroupRequest(Guid groupId, Guid userId);
}
