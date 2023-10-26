using Microsoft.EntityFrameworkCore;
using SimU_GameService.Domain;

namespace SimU_GameService.Infrastructure.Persistence;

public class SimUDbContext : DbContext
{
    public SimUDbContext(DbContextOptions<SimUDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users {get; set; } = null!;
    public DbSet<Chat> Chats {get; set; } = null!;
    public DbSet<Group> Groups {get; set; } = null!;

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SimUDbContext).Assembly);
        modelBuilder.Entity<User>()
                .HasOne(u => u.lastLocation)
                .WithOne()
                .HasForeignKey<User>(u => u.Id); // User.Id is used as the foreign key
    }
}