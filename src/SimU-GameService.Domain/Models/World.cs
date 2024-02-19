using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class World : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string WorldCode { get; set; } = default!;
    public Guid CreatorId { get; set; }
    public DateTime CreatedTime { get; set; }
    public List<Guid> WorldUsers { get; set; }
    public List<Guid> WorldAgents { get; set; }
    public Uri? ThumbnailURL { get; set; }

    public World()
    {
        WorldUsers = new List<Guid>();
        WorldAgents = new List<Guid>();
    }
    public World(string name, string description, Guid ownerId, string worldCode) : this()
    {
        Name = name;
        CreatorId = ownerId;
        Description = description;
        CreatedTime = DateTime.UtcNow;
        WorldCode = worldCode;
        WorldUsers.Add(ownerId);
    }
}