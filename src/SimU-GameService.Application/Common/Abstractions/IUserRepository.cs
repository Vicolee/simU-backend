using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a user to the repository.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task AddUser(User user);

    /// <summary>
    /// Removes a user from the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task RemoveUser(Guid userId);

    /// <summary>
    /// Gets a user from the repository by ID.
    /// </summary>
    /// <param name="userId"</param>
    /// <returns></returns>
    public Task<User?> GetUser(Guid userId);

    /// <summary>
    /// Gets a user from the repository by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<User?> GetUserByEmail(string email);

    /// <summary>
    /// Updates a user's summary.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="summary"></param>
    /// <returns></returns>
    public Task UpdateUserSummary(Guid userId, string summary);

    /// <summary>
    /// Get's the list of worlds a user belongs to
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<IEnumerable<Guid?>> GetUserWorlds(Guid userId);

    /// <summary>
    /// Add's a world to a user's list of worlds that they belong to.
    /// <param name="userId"></param>
    /// <param name="worldId"></param>
    /// <param name="isOwner"></param>
    /// <returns></returns>
    public Task AddWorld(Guid userId, Guid worldId, bool isOwner);

    /// <summary>
    /// Add's a world to a user's list of worlds that they belong to.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="worldId"></param>
    /// <param name="joinCode"></param>
    /// <returns></returns>
    public Task RemoveWorldFromList(Guid userId, Guid worldId);

    /// <summary>
    /// Updates a user's in game sprite / character avatar
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="spriteURL"></param>
    /// <param name="spriteHeadshotURL"></param>
    /// <returns></returns>
    public Task UpdateSprite(Guid userId, Uri spriteURL, Uri spriteHeadshotURL);

    /// <summary>
    /// Updates a user's location in the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="xCoord"></param>
    /// <param name="yCoord"></param>
    /// <returns></returns>
    Task UpdateLocation(Guid userId, int xCoord, int yCoord);
    Task<Location?> GetLocation(Guid locationId);
}
