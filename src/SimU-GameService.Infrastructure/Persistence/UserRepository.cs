using SimU_GameService.Application.Common;
using SimU_GameService.Domain;

namespace SimU_GameService.Infrastructure.Persistence;

/// <summary>
/// This class specifies our database implementation. 
/// For now, we are using a simple in-memory database.
/// To be replaced with an Entity Framework Core implementation.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private IEnumerable<User> Users => _users;

    public Task AddUser(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> GetUserByEmail(string email)
    {
        var result = _users.FirstOrDefault(u => u.email == email);
        return Task.FromResult(result);
    }

    public Task<User?> GetUserById(Guid userId)
    {
        var result = _users.FirstOrDefault(u => u.Id == userId);
        return Task.FromResult(result);
    }

    public Task RemoveUser(Guid userId)
    {
        _users.RemoveAll(u => u.Id == userId);
        return Task.CompletedTask;
    }
}
