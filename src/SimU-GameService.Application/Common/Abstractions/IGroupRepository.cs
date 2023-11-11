using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

public interface IGroupRepository
{
    /// <summary>
    /// Add a group to the database
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    Task AddGroup(Group group);

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
}