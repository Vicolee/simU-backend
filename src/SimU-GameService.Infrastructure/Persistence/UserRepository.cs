using Microsoft.EntityFrameworkCore;
using SimU_GameService.Application.Common;
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
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is not null)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}
