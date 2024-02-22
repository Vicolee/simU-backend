using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly SimUDbContext _dbContext;

    public AgentRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAgent(Agent agent)
    {
        await _dbContext.AddAsync(agent);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Agent?> GetAgent(Guid agentId) => await _dbContext.Agents
        .FirstOrDefaultAsync(a => a.Id == agentId);

    public async Task<string?> GetSummary(Guid agentId)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);
        return agent.Summary;
    }

    public async Task RemoveAgent(Guid agentId)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);
        _dbContext.Remove(agent);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateDescription(Guid agentId, string description)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);
        agent.Description = description;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSprite(Guid agentId, Uri spriteURL, Uri spriteHeadshotURL)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);

        agent.SpriteURL = spriteURL;
        agent.SpriteHeadshotURL = spriteHeadshotURL;

        await _dbContext.SaveChangesAsync();
    }
}
