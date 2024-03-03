using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class WorldRepository : IWorldRepository
{
    private readonly SimUDbContext _dbContext;

    public WorldRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task CreateWorld(World world)
    {
        await _dbContext.AddAsync(world);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<World?> GetWorld(Guid worldId)
    {
        return await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);
    }

    public async Task<User?> GetCreator(Guid worldId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == world.CreatorId);
    }

    public async Task AddUser(Guid worldId, Guid userId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        // checks to see if the user is already added in the world
        if (world.WorldUsers.Contains(userId))
        {
            throw new BadRequestException("User is already added to the world");
        }
        world.WorldUsers.Add(userId);
        await _dbContext.SaveChangesAsync();
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

        if (world.CreatorId == userId)
        {
            throw new BadRequestException("The creator of the world cannot be removed");
        }
        world.WorldUsers.Remove(userId);
        _dbContext.SaveChanges();
    }

    public async Task RemoveAgent(Guid worldId, Guid creatorId, Guid agentId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        var agent = await _dbContext.Agents.FirstOrDefaultAsync(a => a.Id == agentId)
            ?? throw new NotFoundException(nameof(Agent), agentId);

        // the user who requests the removal of an agent must either be
        // the creator of the agent or the owner of world.
        if (world.CreatorId != creatorId && agent?.CreatorId != creatorId)
        {
            throw new BadRequestException("Only the creator of the world or the creator of the agent can remove the agent");
        }
        world.WorldAgents.Remove(agentId);
        _dbContext.SaveChanges();
    }

    public async Task DeleteWorld(Guid worldId, Guid ownerId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        if (world.CreatorId != ownerId)
        {
            throw new BadRequestException("Only the creator of the world of the world can delete it");
        }

        _dbContext.Worlds.Remove(world);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> WorldCodeExists(string worldCode)
        => await _dbContext.Worlds.AnyAsync(w => w.WorldCode == worldCode);

    public async Task<Guid?> GetWorldIdByWorldCode(string worldCode)
    {
        var world = await _dbContext.Worlds.FirstOrDefaultAsync(w => w.WorldCode == worldCode)
            ?? throw new NotFoundException(nameof(World), worldCode);
        return world.Id;
    }

    public async Task UpdateWorldThumbnail(Guid worldId, Uri thumbnailURL)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        world.ThumbnailURL = thumbnailURL;
        await _dbContext.SaveChangesAsync();
    }
}
