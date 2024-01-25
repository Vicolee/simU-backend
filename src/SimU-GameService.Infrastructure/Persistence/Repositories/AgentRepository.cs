using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Common.Abstractions;
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

    public async Task<Agent?> GetAgent(Guid agentId)
    {
        return await _dbContext.Agents
            .FirstOrDefaultAsync(a => a.Id == agentId);
    }

    public async Task<Location?> GetLocation(Guid locationId)
    {
        var agent = await _dbContext.Agents
            .FirstOrDefaultAsync(a => a.Location != null && a.Location.LocationId == locationId);
        return agent?.Location;
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

        var user = _dbContext.Users
            .FirstOrDefault(u => u.UserId == userId) ?? throw new NotFoundException(nameof(User), userId);

        user.QuestionResponses = responses
            .Select((response, index) => new QuestionResponse(_questions.ElementAt(index), response))
            .ToList();

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriend(Guid userId, Guid friendId)
    {
        var user = await GetUser(userId)
            ?? throw new NotFoundException(nameof(User), userId);

        var friend = user.Friends.FirstOrDefault(f => f.FriendId == friendId);

        if (friend is not null)
        {
            user.Friends.Remove(friend);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Friend>> GetFriends(Guid userId)
    {
        var user = await GetUser(userId)
            ?? throw new NotFoundException(nameof(User), userId);

        return user.Friends.AsEnumerable();
    }

    public async Task AddFriend(Guid userId, Guid friendId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        var friend = await GetUser(friendId) ?? throw new NotFoundException(nameof(User), friendId);

        user.Friends.Add(new Friend(friendId, DateTime.UtcNow));
        friend.Friends.Add(new Friend(userId, DateTime.UtcNow));
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLocation(Guid userId, int xCoord, int yCoord)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);

        user.UpdateLocation(xCoord, yCoord);
        await _dbContext.SaveChangesAsync();
    }

}
