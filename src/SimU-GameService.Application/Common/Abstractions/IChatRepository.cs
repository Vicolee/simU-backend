using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// </summary>
public interface IChatRepository
{
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
}