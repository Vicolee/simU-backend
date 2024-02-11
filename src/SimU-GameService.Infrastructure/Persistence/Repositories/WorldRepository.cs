using System.Runtime.InteropServices;
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
            .FirstOrDefaultAsync(w => w.Id == worldId) ?? throw new NotFoundException(nameof(World), worldId);
    }

    public async Task<User?> GetWorldCreator(Guid worldId)
    {
        var world = await GetWorld(worldId);

        if (world == null)
        {
            return null;
        }

        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == world.OwnerId);
    }

    public async Task<Unit> AddUserToWorld(Guid worldId, Guid userId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        world.UsersInWorld.Add(userId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<Unit> AddAgentToWorld(Guid worldId, Guid agentId)
    {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        world.UsersInWorld.Add(agentId);
        _dbContext.SaveChanges();
        return Unit.Value;
    }

    public async Task<IEnumerable<User?>?> GetWorldUsers(Guid worldId) {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

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
       var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

        var agents = new List<Agent?>();
        foreach (var agentId in world.AgentsInWorld)
        {
            var agent = await _dbContext.Agents
                .FirstOrDefaultAsync(a => a.Id == agentId);
            agents.Add(agent);
        }

        return agents;
    }

    public async Task RemoveUserFromWorld(Guid worldId, Guid userId, Guid ownerId) {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

        // if user being removed is the owner, throw an error
        if (world.OwnerId == userId)
        {
            throw new BadRequestException("Owner cannot be removed from the world");
        }

        // if the user requesting the removal of another is not the owner, throw an error
        if (world.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not the owner of this world; you lack permission to remove a user from the world");
        }
        world.UsersInWorld.Remove(userId);
        _dbContext.SaveChanges();
    }

    public async Task RemoveAgentFromWorld(Guid worldId, Guid agentId, Guid deleterId) {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);

        // grabs the agent so we can gets its creator's ID
        var agent = await _dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == agentId);

        // the user who requests the removal of an agent must either be:
        // 1. the creator of the agent OR
        // 2. the owner of world.
        if (world.OwnerId != deleterId || agent?.CreatedByUser != deleterId)
        {
            throw new UnauthorizedAccessException("You lack the necessary permissions to remove this agent from the world; only the creator of the agent or owner of the world may remove it");
        }
        world.AgentsInWorld.Remove(agentId);
        _dbContext.SaveChanges();
    }

    public async Task DeleteWorld(Guid worldId, Guid ownerId) {
        var world = await GetWorld(worldId) ?? throw new NotFoundException(nameof(World), worldId);
        if (world.OwnerId != ownerId)
        {
            throw new BadRequestException("Only the owner of the world can delete the world");
        }

        _dbContext.Worlds.Remove(world);
        _dbContext.SaveChanges();
    }

    public async Task<bool> JoinCodeExists(string joinCode)
    {
        return await _dbContext.Worlds
            .AnyAsync(w => w.JoinCode == joinCode);
    }

    public async Task<Guid?> MatchJoinCodeToWorldId(string joinCode)
    {
        var world = await _dbContext.Worlds
            .FirstOrDefaultAsync(w => w.JoinCode == joinCode);
        if (world == null)
        {
            return null;
        } else {
            return world.Id;
        }
    }

}
