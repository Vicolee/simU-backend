using GameService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameService.Persistence;

public class SimUDbContext : DbContext
{
    public SimUDbContext(DbContextOptions<SimUDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users {get; set; } = null!;
    public DbSet<Chat> Chats {get; set; } = null!;

    public DbSet<Group> Groups {get; set; } = null!;
}