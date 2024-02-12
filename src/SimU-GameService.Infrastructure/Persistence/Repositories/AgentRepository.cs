using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly SimUDbContext _dbContext;

    public AgentRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAgent(Agent agent)
    {
        _dbContext.Agents.Add(agent);
        await _dbContext.SaveChangesAsync();
    }

    // TODO: replace hard-coded sample questions with a database table.
    private readonly IEnumerable<string> _questions = new List<string>()
    {
        "What is your favorite color?",
        "What is your favorite animal?",
        "What is your favorite food?",
        "What is your favorite movie?",
        "What is your favorite book?",
        "What is your favorite song?",
        "What is your favorite game?",
        "What is your favorite sport?",
        "What is your favorite hobby?",
        "What is your favorite place?"
    };

    public Task<IEnumerable<string>> GetQuestions() => Task.FromResult(_questions);

    public async Task<Agent?> GetAgent(Guid agentId) => await _dbContext.Agents
        .FirstOrDefaultAsync(a => a.Id == agentId);

    public async Task<string?> GetSummary(Guid agentId)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);
        return agent.Summary;
    }

    public async Task<Location?> GetLocation(Guid agentId)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);
        return agent.Location;
    }

    public async Task RemoveAgent(Guid agentId)
    {
        var agent = await GetAgent(agentId);

        if (agent is null)
        {
            return;
        }
        _dbContext.Agents.Remove(agent);
        _dbContext.SaveChanges();
    }

    public async Task PostResponses(Guid agentId, IEnumerable<string> responses)
    {
        if (responses.Count() != _questions.Count())
        {
            throw new ArgumentException("The number of responses must match the number of questions.");
        }

        var agent = _dbContext.Agents
            .FirstOrDefault(a => a.Id == agentId) ?? throw new NotFoundException(nameof(Agent), agentId);

        // TODO: flesh out this logic

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLocation(Guid agentId, int xCoord, int yCoord)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);

        agent.UpdateLocation(xCoord, yCoord);
        await _dbContext.SaveChangesAsync();
    }

}