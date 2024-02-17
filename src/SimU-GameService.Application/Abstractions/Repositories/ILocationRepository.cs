using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

public interface ILocationRepository
{
    /// <summary>
    /// Gets a character's location from the database by ID.
    /// The character can be a user or an agent.
    /// </summary>
    /// <param name="characterId">The ID of the user/agent</param>
    /// <param name="isAgent">Whether the character is an agent or not</param>
    /// <returns> A <see cref="Location"/> object</returns>
    Task<Location?> GetLocation(Guid characterId, bool isAgent = false);

    /// <summary>
    /// Updates a character's location in the database.
    /// </summary>
    /// <param name="characterId">The ID of the user/agent</param>
    /// <param name="x_coord">The new X coordinate</param>
    /// <param name="y_coord">The new Y coordinate</param>
    /// <param name="isAgent">Whether the character is an agent or not</param>
    /// <returns></returns>
    Task UpdateLocation(Guid characterId, int x_coord, int y_coord, bool isAgent = false);
}