using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class User : Entity
{
    public Guid UserId { get; private set; }
    public string? IdentityId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool IsAgent { get; set; } = false;
	public string? Description { get; set; }
    public DateTime CreatedTime { get; set; }
    public Location? Location { get; set; }
    public List<Friend> Friends { get; set; }
    public List<QuestionResponse> QuestionResponses { get; set; }

    public User() : base()
    {
        Friends = new();
        QuestionResponses = new();
        Location = new Location {
            LocationId = Guid.NewGuid(),
            X = 0,
            Y = 0,
        };
        UserId = Id;
    }

    public User(
        string identityId,
        string username,
        string email,
        bool isAgent,
        string description
        ) : this()
    {
        IdentityId = identityId;
        Username = username;
        Email = email;
        IsAgent = isAgent;
        Description = description;
        CreatedTime = DateTime.UtcNow;
    }

    public void UpdateLocation(int xCoord, int yCoord)
    {
        Location = new Location
        {
            LocationId = Guid.NewGuid(),
            X = xCoord,
            Y = yCoord
        };
    }
}