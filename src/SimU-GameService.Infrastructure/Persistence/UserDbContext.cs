using GameService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameService.Persistence;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users {get; set; } = null!;
}