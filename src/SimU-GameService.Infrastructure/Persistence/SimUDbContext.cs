using Microsoft.EntityFrameworkCore;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence;

public class SimUDbContext : DbContext
{
    public SimUDbContext(DbContextOptions<SimUDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SimUDbContext).Assembly);
    }
}