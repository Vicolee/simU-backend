using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly SimUDbContext _dbContext;

    public UserRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddUser(User user)
    {
        _dbContext.Add(user);
        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }

    // hard-coded sample questions for the entrance questionnaire.
    // TODO: replace with a database table.
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

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task RemoveUser(Guid userId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return;
        }
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    public async Task PostResponses(Guid userId, IEnumerable<string> responses)
    {
        var user = _dbContext.Users
            .FirstOrDefault(u => u.Id == userId) ?? throw new Exception($"User with ID {userId} not found.");

        user.QuestionResponses = responses.ToList();
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriend(Guid userId, Guid friendId)
    {
        var user = await GetUserById(userId)
            ?? throw new Exception($"User with ID {userId} not found.");

        var friend = user.Friends.FirstOrDefault(f => f.FriendId == friendId);

        if (friend is not null)
        {
            user.Friends.Remove(friend);
            await _dbContext.SaveChangesAsync();
        }
    }
}
