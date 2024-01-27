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

    public async Task UpdateLocation(Guid userId, int xCoord, int yCoord)
    {
        var user = await GetUser(userId) ?? throw new NotFoundException(nameof(User), userId);

        user.UpdateLocation(xCoord, yCoord);
        await _dbContext.SaveChangesAsync();
    }

}
