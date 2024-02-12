using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

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
    /// Gets a user's summary that was generated by chat GPT according
    /// to their responses to the questions we asked them.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<string?> GetUserSummary(Guid userId);

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
    public Task<IEnumerable<Guid>> GetUserWorlds(Guid userId);

    /// <summary>
    /// Adds a world to a user's list of worlds that they belong to.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="worldId"></param>
    /// <returns></returns>
    public Task AddUserToWorld(Guid userId, Guid worldId);
    
    public Task RemoveUserFromWorld(Guid userId, Guid worldId);

    /// <summary>
    /// Updates a user's location in the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="xCoord"></param>
    /// <param name="yCoord"></param>
    /// <returns></returns>
    Task UpdateLocation(Guid userId, int xCoord, int yCoord);

    // TODO: why are we passing locationId here?
    Task<Location?> GetLocation(Guid locationId);
    Task RemoveFriend(Guid userId, Guid friendId);
    Task AddFriend(Guid requesterId, Guid requesteeId);
    Task PostResponses(Guid userId, IEnumerable<string> responses);
    Task<Guid> GetUserFromIdentityId(string identityId);
}
