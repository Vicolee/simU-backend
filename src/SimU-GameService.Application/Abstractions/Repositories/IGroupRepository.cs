using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

public interface IGroupRepository
{
    /// <summary>
    /// Add a group to the database
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    Task AddGroup(Group group);

    /// <summary>
    /// Add a user with ID <paramref name="userId"/> to the group with ID <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task AddUser(Guid groupId, Guid userId);

    /// <summary>
    /// Delete a group from the database
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task DeleteGroup(Guid groupId);

    /// <summary>
    /// Get a group from the database
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<Group?> GetGroup(Guid groupId);

    /// <summary>
    /// Remove a user with ID <paramref name="userId"/> from the group with ID <paramref name="groupId"/>
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task RemoveUser(Guid groupId, Guid userId);
}