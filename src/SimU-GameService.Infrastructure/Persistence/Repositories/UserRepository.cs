using MediatR;
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

    public Task<bool> AddWorld(Guid userId, Guid worldId, bool isOwner)
    {
        var user = _dbContext.Users
            .FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            if (isOwner)
            {
                user.WorldsCreated = user.WorldsCreated.Append(worldId).ToList();
            } else {
                user.WorldsJoined = user.WorldsJoined.Append(worldId).ToList();
            }
            _dbContext.SaveChanges();
            return Task.FromResult(true);
        }
        return Task.FromResult(true);
    }
    public async Task<User?> GetUser(Guid userId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Location?> GetLocation(Guid locationId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Location != null && u.Location.LocationId == locationId);
        return user?.Location;
    }

    public async Task<IEnumerable<Guid>> GetUserWorlds(Guid userId)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);
        var userWorlds = new List<Guid>();
        userWorlds.AddRange(user.WorldsJoined);
        userWorlds.AddRange(user.WorldsCreated);
        return userWorlds;
    }

    public async Task UpdateUserSummary(Guid userId, string summary)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);

        user.Summary = summary;
        await _dbContext.SaveChangesAsync();
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

    public async Task RemoveWorldFromList(Guid userId, Guid worldId)
    {
        var user = await GetUser(userId);

        if (user is null)
        {
            return;
        }
        user.WorldsJoined = user.WorldsJoined.Where(w => w != worldId).ToList();
        _dbContext.SaveChanges();
    }

    public async Task UpdateUserSprite(Guid userId, Uri spriteURL, Uri spriteHeadshotURL)
    {
       var user = await GetUser(userId);
       if (user is null)
       {
           return;
       }
        user.Sprite = spriteURL;
        user.SpriteHeadshot = spriteHeadshotURL;
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLocation(Guid userId, int xCoord, int yCoord)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);

        user.UpdateLocation(xCoord, yCoord);
        await _dbContext.SaveChangesAsync();
    }

}
