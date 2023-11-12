using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class User : Entity
{
    public string? IdentityId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool IsAgent { get; set; } = false;
	public string? Description { get; set; }
    public DateTime CreatedTime { get; set; }
    public List<Guid> ChatIds { get; set; }
    public List<Friend> Friends { get; set; }
    public Location? Location { get; set; }
    public List<string> QuestionResponses { get; set; }

    // TODO: implement models for Memories and Personality later
    // public IEnumerable<Memory> Memories { get; set; }


    public User() : base()
    {
        ChatIds = new();
        Friends = new();
        QuestionResponses = new();
    }

    public User(
        string identityId,
        string firstName,
        string lastName,
        string email,
        bool isAgent,
        string description
        ) : this()
    {
        IdentityId = identityId;
        FirstName = firstName;
        LastName = lastName;
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