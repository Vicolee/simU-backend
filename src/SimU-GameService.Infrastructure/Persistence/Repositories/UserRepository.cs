using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

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

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUser(Guid userId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Location?> GetLocation(Guid locationId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Location != null && u.Location.LocationId == locationId);
        return user?.Location;
    }

    public async Task RemoveUser(Guid userId)
    {
        var user = await GetUser(userId);

        if (user is null)
        {
            return;
        }
        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
    }

    public async Task PostResponses(Guid userId, IEnumerable<string> responses)
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
