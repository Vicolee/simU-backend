using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
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

    public Task<Location?> GetLocation(Guid locationId)
    {
        // TODO: again, why exactly are we using the locationId here?
        throw new NotImplementedException();
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

    public Task PostResponses(Guid userId, IEnumerable<string> responses)
    {
        throw new NotImplementedException();
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

    public Task<string?> GetUserSummary(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserSummary(Guid userId, string summary)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<World?>> GetUserWorlds(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task AddUserWorld(Guid userId, Guid worldId, string joinCode)
    {
        throw new NotImplementedException();
    }
}
