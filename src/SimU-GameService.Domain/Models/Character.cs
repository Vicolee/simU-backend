using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Character : Entity
{
    public string Username { get; set; } = default!;
    public string? Summary { get; set; }
    public DateTime CreatedTime { get; set; }
    public Location? Location { get; set; }

    protected Character() : base()
    {
    }

    protected Character(string username, string summary)
        : this()
    {
        Username = username;
        Summary = summary;
        CreatedTime = DateTime.UtcNow;
        Location = new Location(0, 0);
    }

    public void UpdateLocation(int xCoord, int yCoord)
    {
        Location = new Location(xCoord, yCoord);
    }
}