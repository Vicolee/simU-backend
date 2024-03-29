using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// </summary>
public interface IChatRepository
{
    /// <summary>
    /// Adds a chat to the repository.
    /// </summary>
    /// <param name="chat"></param>
    /// <returns></returns>
    Task AddChat(Chat chat);

    /// <summary>
    /// Deletes a chat from the repository by ID.
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    Task DeleteChat(Guid chatId);

    /// <summary>
    /// Gets a chat from the repository by ID.
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    Task<Chat?> GetChat(Guid chatId);

    /// <summary>
    /// Get all chats sent or received by a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Chat>> GetChatsByUserId(Guid userId);

    /// <summary>
    /// Gets all chat messages sent by a user to another user.
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="recipientId"></param>
    /// <returns></returns>
    Task<IEnumerable<Chat>> GetChatsBySenderAndReceiverIds(Guid senderId, Guid recipientId);
}