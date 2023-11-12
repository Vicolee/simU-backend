namespace SimU_GameService.Api.Hubs.Abstractions;

/// <summary>
/// This interface specifies the methods on the server that can be invoked by the client.
/// </summary>
public interface IUnityHub
{
    /// <summary>
    /// Updates the location of the user to the given <paramref name="x_coord"/> and <paramref name="y_coord"/>
    /// </summary>
    /// <param name="x_coord"></param>
    /// <param name="y_coord"></param>
    /// <returns></returns>
    Task UpdateLocation(int x_coord, int y_coord);

    /// <summary>
    /// Sends a friend request to the user with the given <paramref name="userId"/>
    /// </summary>
    /// <param name="userId"> The target of the friend request</param>
    /// <returns></returns>
    Task SendFriendRequest(Guid userId);

    /// <summary>
    /// Responds to a friend request from the user with the given <paramref name="userId"/>
    /// </summary>
    /// <param name="userId"> The sender of the friend request</param>
    /// <param name="accepted"> Indicates whether the friend request has been accepted or not</param>
    /// <returns></returns>
    Task RespondToFriendRequest(Guid userId, bool accepted);

    /// <summary>
    /// Requests for a user to be added to a group with the given <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId">The ID of the target group</param>
    /// <returns></returns>
    Task RequestToJoinGroup(Guid groupId);

    /// <summary>
    /// Adds a user to a group with the given <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId">The ID of the target group</param>
    /// <param name="userId">The ID of the user being added to the group</param>
    /// <returns></returns>
    Task AddUserToGroup(Guid groupId, Guid userId);

    /// <summary>
    /// Removes a user from a group with the given <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId">The ID of the target group</param>
    /// <param name="userId">The ID of the user being removed from the the group</param>
    /// <returns></returns>
    Task RemoveUserFromGroup(Guid groupId, Guid userId);

    /// <summary>
    /// Sends a <paramref name="message"/> to the user with the given <paramref name="receiverId"/>
    /// </summary>
    /// <param name="receiverId">The ID of the message target</param>
    /// <param name="message">The actual contents of the message</param>
    /// <returns></returns>
    Task SendChat(Guid receiverId, string message);
}