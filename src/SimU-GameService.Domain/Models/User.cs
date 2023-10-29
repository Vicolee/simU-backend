using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class User : Entity
{
    public Guid IdentityId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedTime { get; set; }
    public List<Guid> ChatIds { get; set; }
    public List<Friend> Friends { get; set; }
    public Location? LastKnownLocation { get; set; }

    // TODO: implement models for Memories and Personality later
    // public IEnumerable<Memory> Memories { get; set; }
    // public Personality personality { get; set; }

    public User() : base()
    {
        ChatIds = new();
        Friends = new();
    }

    public User(
        Guid identityId,
        string firstName,
        string lastName,
        string email) : this()
    {
        IdentityId = identityId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}