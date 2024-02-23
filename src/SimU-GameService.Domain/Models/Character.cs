using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Character : Entity
{
    public string Username { get; set; } = default!;
    public string? Summary { get; set; }
    public DateTime CreatedTime { get; set; }
    public Location? Location { get; set; }
    public List<Response> QuestionResponses { get; set; }

    protected Character() : base()
    {
        QuestionResponses = new();
    }

    protected Character(string username, string summary)
        : this()
    {
        Username = username;
        Summary = summary;
        CreatedTime = DateTime.UtcNow;
    }

    public void UpdateLocation(int xCoord, int yCoord)
    {
        Location = new Location(xCoord, yCoord);
    }
}