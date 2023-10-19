namespace SimU_GameService.Api.Hubs;

/// <summary>
/// Defines the methods that can be invoked by the <see cref="UnityClientHub"/>
/// </summary>
public interface IUnityClient
{
    // TODO: Add client methods to be invoked by the server here.
    // Here is an example:

    /// <summary>
    /// Broadcasts a message to all connected clients.
    /// </summary>
    /// <param name="user">The sender of the message</param>
    /// <param name="message">The content of the message</param>
    /// <returns></returns>
    Task ReceiveMessage(string user, string message);
}
