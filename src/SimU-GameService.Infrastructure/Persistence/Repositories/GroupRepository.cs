using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly SimUDbContext _dbContext;

    public GroupRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddGroup(Group group)
    {
        await _dbContext.Groups.AddAsync(group);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteGroup(Guid groupId)
    {
        var group = _dbContext.Groups.Find(groupId);
        
        if (group is not null)
        {
            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Group?> GetGroup(Guid groupId)
    {
        return await _dbContext.Groups.FindAsync(groupId);
    }
}