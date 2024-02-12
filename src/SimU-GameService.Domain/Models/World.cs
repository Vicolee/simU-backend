using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class World : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid CreatorId { get; set; }
    public DateTime CreatedTime { get; set; }
    public List<Guid> WorldUsers { get; set; }
    public List<Guid> WorldAgents { get; set; }
    public Uri? ThumbnailURL { get; set; }
    public string? WorldCode { get; set; }

    public World()
    {
        WorldUsers = new();
        WorldAgents = new();
    }

    public World(string name, string description, Guid ownerId) : this()
    {
        Name = name;
        CreatorId = ownerId;
        Description = description;
        CreatedTime = DateTime.UtcNow;
        WorldCode = GenerateWorldCode();
    }

    private static string GenerateWorldCode()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}