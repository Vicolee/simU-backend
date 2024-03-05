using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SimUDbContext _dbContext;

    public UserRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task AddUser(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUser(Guid userId) 
        => await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User?> GetUserByEmail(string email) => await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddUserToWorld(Guid userId, Guid worldId, bool isOwner)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        if (isOwner)
        {
            // checks to see if the worldId is already added to the user's list of worlds created or joined to avoid adding duplicates
            if (user.WorldsCreated.Contains(worldId))
            {
                throw new BadRequestException("The user (owner of the world) already has the world added to their list of worlds created.");
            }
            user.WorldsCreated.Add(worldId);
        } else {
            // checks to see if the worldId is already added to the user's list of worlds created or joined to avoid adding duplicates
            if (user.WorldsJoined.Contains(worldId)) {
                throw new BadRequestException("The user already has the world added to their list of worlds joined.");
            }
            user.WorldsJoined.Add(worldId);
        }
        user.ActiveWorldId = worldId;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Guid>> GetUserWorlds(Guid userId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        var userWorlds = new List<Guid>();
        userWorlds.AddRange(user.WorldsJoined);
        userWorlds.AddRange(user.WorldsCreated);
        return userWorlds;
    }

    public async Task RemoveUser(Guid userId)
    {
        var user = await GetUser(userId);
        if (user is not null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveWorldFromList(Guid userId, Guid worldId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        user.WorldsJoined.Remove(worldId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUserSprite(Guid userId, List<int> animations)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);

        user.SpriteAnimations = animations;

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

    public async Task<string?> GetUserSummary(Guid userId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        return user.Summary;
    }

    public async Task UpdateUserSummary(Guid userId, string summary)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        user.Summary = summary;
        await _dbContext.SaveChangesAsync();
    }

    public Task<Guid> GetUserIdFromIdentityId(string identityId) => _dbContext.Users
            .Where(u => u.IdentityId == identityId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();

    public async Task RemoveUserFromWorld(Guid userId, Guid worldId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        user.WorldsJoined.Remove(worldId);
        if (user.ActiveWorldId == worldId)
        {
            user.ActiveWorldId = default;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task Login(Guid userId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        user.Login();
        await _dbContext.SaveChangesAsync();
    }

    public async Task Logout(Guid userId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        user.Logout();
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string?> GetIdentityIdFromUserId(Guid userId)
    {
        var user = await GetUser(userId);
        return user?.IdentityId ?? default;
    }

    public async Task<IEnumerable<string>> GetOnlineUsers() => await _dbContext.Users
            .Where(u => u.IsOnline)
            .Select(u => u.IdentityId)
            .ToListAsync();
}