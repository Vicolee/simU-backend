using MediatR;
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

    public Task AddAgent(Agent agent)
    {
        _dbContext.Add(agent);
        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }

    public async Task<Agent?> GetAgent(Guid agentId)
    {
        return await _dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == agentId);
    }

    public async Task<object?> GetSummary(Guid agentId)
    {
        var agent = await _dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == agentId);
        return agent?.Summary;
    }
    public Task<Location?> GetLocation(Guid locationId)
    {
        // TODO: why are we using the locationId here?
        throw new NotImplementedException();
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

    public async Task<Unit> UpdateAgentSprite(Guid agentId, Uri spriteURL, Uri spriteHeadshotURL)
    {
       var agent = await GetAgent(agentId);
       if (agent is null)
       {
           return Unit.Value;
       }
        agent.SpriteURL = spriteURL;
        agent.SpriteHeadshotURL = spriteHeadshotURL;
        await _dbContext.SaveChangesAsync();
        return Unit.Value;
    }

    public async Task UpdateLocation(Guid agentId, int xCoord, int yCoord)
    {
        var agent = await GetAgent(agentId) ?? throw new NotFoundException(nameof(Agent), agentId);

        agent.UpdateLocation(xCoord, yCoord);
        await _dbContext.SaveChangesAsync();
    }


    // public Task<IEnumerable<string>> GetQuestions() => Task.FromResult(_questions);
    
    // // TODO: replace hard-coded sample questions with a database table.
    // private readonly IEnumerable<string> _questions = new List<string>()
    // {
    //     "What is your favorite color?",
    //     "What is your favorite animal?",
    //     "What is your favorite food?",
    //     "What is your favorite movie?",
    //     "What is your favorite book?",
    //     "What is your favorite song?",
    //     "What is your favorite game?",
    //     "What is your favorite sport?",
    //     "What is your favorite hobby?",
    //     "What is your favorite place?"
    // };

    // public async Task PostResponses(Guid agentId, IEnumerable<string> responses)
    // {
    //     if (responses.Count() != _questions.Count())
    //     {
    //         throw new ArgumentException("The number of responses must match the number of questions.");
    //     }

    //     var agent = _dbContext.Agents
    //         .FirstOrDefault(a => a.Id == agentId) ?? throw new NotFoundException(nameof(Agent), agentId);

    //     agent.QuestionResponses = responses
    //         .Select((response, index) => new QuestionResponse(_questions.ElementAt(index), response))
    //         .ToList();

    //     await _dbContext.SaveChangesAsync();
    // }

}
