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
            .FirstOrDefaultAsync(u => u.Id == world.OwnerId);
    }

    public async Task<Unit> AddUserToWorld(Guid worldId, Guid userId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        world.UsersInWorld.Add(userId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<Unit> AddAgentToWorld(Guid worldId, Guid agentId)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        world.UsersInWorld.Add(agentId);
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
        foreach (var userId in world.UsersInWorld)
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
        foreach (var agentId in world.AgentsInWorld)
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

        if (world.OwnerId == userId)
        {
            throw new BadRequestException("Owner cannot be removed from the world");
        }

        world.UsersInWorld.Remove(userId);
        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }

    public Task DeleteWorld(Guid worldId, Guid ownerId) {
        var world = _dbContext.Worlds
            .FirstOrDefault(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);

        if (world.OwnerId != ownerId)
        {
            throw new BadRequestException("Only the owner can delete the world");
        }

        _dbContext.Worlds.Remove(world);
        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }

}
