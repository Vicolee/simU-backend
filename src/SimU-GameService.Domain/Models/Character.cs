using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Character : Entity
{
    public string? Username { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public DateTime CreatedTime { get; set; }
    public Location? Location { get; set; }
    public Uri? SpriteURL { get; set; }
    public Uri? SpriteHeadshotURL { get; set; }
    public List<Response> QuestionResponses { get; set; }

    protected Character() : base()
    {
        QuestionResponses = new();
    }

    protected Character(string username, string summary, Uri spriteURL, Uri spriteHeadshotURL)
        : this()
    {
        Username = username;
        Summary = summary;
        CreatedTime = DateTime.UtcNow;
        SpriteURL = spriteURL;
        SpriteHeadshotURL = spriteHeadshotURL;
    }

    public void UpdateLocation(int xCoord, int yCoord)
    {
        Location = new Location(xCoord, yCoord);
    }
}