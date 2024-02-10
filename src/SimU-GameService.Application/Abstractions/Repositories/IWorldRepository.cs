using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IWorldRepository
{
    /// <summary>
    /// Adds a new world to the repository.
    /// </summary>
    /// <param name="world"></param>
    /// <returns></returns>
    public Task CreateWorld(World world);

    /// <summary>
    /// Gets a world from the repository by ID.
    /// </summary>
    /// <param name="worldId"</param>
    /// <returns></returns>
    public Task<World?> GetWorld(Guid worldId);

    /// <summary>
    /// Gets a world from the repository by ID.
    /// </summary>
    /// <param name="worldId"</param>
    /// <returns></returns>
    public Task<User?> GetCreator(Guid worldId);

    /// <summary>
    /// Adds a user to world's list of users
    /// </summary>
    /// <param name="worldId"</param>
    /// <param name="userId"</param>
    /// <returns></returns>
    public Task<Unit> AddUser(Guid worldId, Guid userId);

    /// <summary>
    /// Adds a user to world's list of users
    /// </summary>
    /// <param name="worldId"</param>
    /// <param name="agentId"</param>
    /// <returns></returns>
    public Task<Unit> AddAgent(Guid worldId, Guid agentId);

    /// <summary>
    /// Gets a list of the users (both online and offline) in the world.
    /// </summary>
    /// <param name="worldId"</param>
    /// <returns></returns>
    public Task<IEnumerable<User?>?> GetWorldUsers(Guid worldId);

    /// <summary>
    /// Gets a list of the agents in the world.
    /// </summary>
    /// <param name="worldId"</param>
    /// <returns></returns>
    public Task<IEnumerable<Agent?>?> GetWorldAgents(Guid worldId);

    /// <summary>
    /// Kicks a user from the world
    /// </summary>
    /// <param name="worldId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task RemoveUser(Guid worldId, Guid userId);

    /// <summary>
    /// Removes a world from the repository
    /// </summary>
    /// <param name="worldId"></param>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public Task DeleteWorld(Guid worldId, Guid ownerId);
}
