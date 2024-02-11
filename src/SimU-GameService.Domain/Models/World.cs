using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class World : Entity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
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

    public World(string name, string description, Guid ownerId, string worldCode) : this()
    {
        Name = name;
        CreatorId = ownerId;
        Description = description;
        CreatedTime = DateTime.UtcNow;
        WorldCode = worldCode;
    }
}