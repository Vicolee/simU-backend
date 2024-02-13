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
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);
    }

    public async Task<User?> GetCreator(Guid worldId)
    {
        var world = await GetWorld(worldId);

        if (world == null)
        {
            return null;
        }

        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == world.CreatorId);
    }

    public async Task<World> AddUser(Guid worldId, Guid userId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        world.WorldUsers.Add(userId);
        await _dbContext.SaveChangesAsync();
        return world;
    }

    public async Task AddAgent(Guid worldId, Guid agentId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        world.WorldAgents.Add(agentId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetWorldUsers(Guid worldId)
    {
        var users = new List<User>();
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

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
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

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

    public async Task RemoveUser(Guid worldId, Guid creatorId, Guid userId)
    {
         var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

        if (world.CreatorId != creatorId)
        {
            throw new BadRequestException("Only the creator of the world can remove users");
        }

        // if user being removed is the owner, throw an error
        if (world.CreatorId == userId)
        {
            throw new BadRequestException("The creator of the world cannot be removed");
        }
        world.WorldUsers.Remove(userId);
        _dbContext.SaveChanges();
    }

    public async Task RemoveAgent(Guid worldId, Guid agentId, Guid deleterId) {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

        // grabs the agent so we can gets its creator's ID
        var agent = await _dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == agentId);

        // the user who requests the removal of an agent must either be:
        // 1. the creator of the agent OR
        // 2. the owner of world.
        if (world.CreatorId != deleterId || agent?.CreatorId != deleterId)
        {
            throw new UnauthorizedAccessException("You lack the necessary permissions to remove this agent from the world; only the creator of the agent or owner of the world may remove it");
        }
        world.WorldAgents.Remove(agentId);
        _dbContext.SaveChanges();
    }

    public async Task DeleteWorld(Guid worldId, Guid ownerId) {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        if (world.CreatorId != ownerId)
        {
            throw new BadRequestException("Only the creator of the world of the world can delete it");
        }

        _dbContext.Worlds.Remove(world);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> WorldCodeExists(string worldCode)
    {
        return await _dbContext.Worlds
            .AnyAsync(w => w.WorldCode == worldCode);
    }

    public async Task<Guid?> MatchWorldCodeToWorldId(string worldCode)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.WorldCode == worldCode);
        if (world == null)
        {
            return null;
        } else {
            return world.Id;
        }
    }

}
