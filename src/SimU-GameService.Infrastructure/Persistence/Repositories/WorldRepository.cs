using MediatR;
using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class WorldRepository : IWorldRepository
{
    private readonly SimUDbContext _dbContext;

    public WorldRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CreateWorld(World world)
    {
        _dbContext.Add(world);
        _dbContext.SaveChangesAsync();
        return Task.CompletedTask;
    }

    public async Task<World?> GetWorld(Guid worldId)
    {
        return await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId);
    }

    public async Task<User?> GetCreator(Guid worldId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId);

        if (world == null)
        {
            return null;
        }

        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == world.CreatorId);
    }

    public async Task<Unit> AddUser(Guid worldId, Guid userId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        world.WorldUsers.Add(userId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<Unit> AddAgent(Guid worldId, Guid agentId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        world.WorldUsers.Add(agentId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<IEnumerable<User>> GetWorldUsers(Guid worldId)
    {
        var users = new List<User>();
        var world = await _dbContext.Worlds.FirstOrDefaultAsync(w => w.Id == worldId)
            ?? throw new NotFoundException(nameof(World), worldId);

        foreach (var userId in world.WorldUsers)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                users.Add(user);
            }
        }
        return users;
    }

    public async Task<IEnumerable<Agent>> GetWorldAgents(Guid worldId)
    {
        var agents = new List<Agent>();
        var world = await _dbContext.Worlds.FirstOrDefaultAsync(w => w.Id == worldId)
            ?? throw new NotFoundException(nameof(World), worldId);

        foreach (var agentId in world.WorldAgents)
        {
            var agent = await _dbContext.Agents.FirstOrDefaultAsync(a => a.Id == agentId);
            if (agent != null)
            {
                agents.Add(agent);
            }
        }
        return agents;
    }

    public Task RemoveUser(Guid worldId, Guid creatorId, Guid userId)
    {
        var world = _dbContext.Worlds
            .FirstOrDefault(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        if (world.CreatorId != creatorId)
        {
            throw new BadRequestException("Only the creator of the world can remove users");
        }

        if (world.CreatorId == userId)
        {
            throw new BadRequestException("The creator of the world cannot be removed");
        }

        world.WorldUsers.Remove(userId);
        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }

    public Task DeleteWorld(Guid worldId, Guid creatorId)
    {
        var world = _dbContext.Worlds
            .FirstOrDefault(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        if (world.CreatorId != creatorId)
        {
            throw new BadRequestException("Only the creator of the world can delete it");
        }

        _dbContext.Worlds.Remove(world);
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
}
