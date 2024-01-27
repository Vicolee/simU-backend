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
    /// Updates a user's location in the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="xCoord"></param>
    /// <param name="yCoord"></param>
    /// <returns></returns>
    Task UpdateLocation(Guid userId, int xCoord, int yCoord);
    Task<Location?> GetLocation(Guid locationId);
}
