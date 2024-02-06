using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Common.Abstractions;
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
        try
        {
            _dbContext.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Failed to create world");
        }

        return Task.CompletedTask;
    }

    public async Task<World?> GetWorld(Guid worldId)
    {
        return await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId);
    }

    public async Task<User?> GetWorldCreator(Guid worldId)
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

    public async Task<Unit> AddUserToWorld(Guid worldId, Guid userId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        world.WorldUsers.Add(userId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<Unit> AddAgentToWorld(Guid worldId, Guid agentId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        world.WorldUsers.Add(agentId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<IEnumerable<User?>?> GetWorldUsers(Guid worldId) {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId);

        if (world == null)
        {
            return null;
        }

        var users = new List<User?>();
        foreach (var userId in world.WorldUsers)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            users.Add(user);
        }

        return users;
    }

    public async Task<IEnumerable<Agent?>?> GetWorldAgents(Guid worldId) {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId);

        if (world == null)
        {
            return null;
        }

        var agents = new List<Agent?>();
        foreach (var agentId in world.WorldAgents)
        {
            var agent = await _dbContext.Agents
                .FirstOrDefaultAsync(a => a.Id == agentId);
            agents.Add(agent);
        }

        return agents;
    }

    public Task RemoveUserFromWorld(Guid worldId, Guid userId) {
        var world = _dbContext.Worlds
            .FirstOrDefault(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        if (world.CreatorId == userId)
        {
            throw new BadRequestException("Owner cannot be removed from the world");
        }

        world.WorldUsers.Remove(userId);
        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }

    public Task DeleteWorld(Guid worldId, Guid ownerId) {
        var world = _dbContext.Worlds
            .FirstOrDefault(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        if (world.CreatorId != ownerId)
        {
            throw new BadRequestException("Only the owner can delete the world");
        }

        _dbContext.Worlds.Remove(world);
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

}
