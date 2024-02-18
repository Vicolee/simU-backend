namespace SimU_GameService.Domain.Models;

public class User : Character
{
    public string IdentityId { get; set; } = default!;
    public string? Email { get; set; }
    public bool IsOnline { get; set; } = false;
    public List<Friend> Friends { get; set; }
    public List<Guid> WorldsJoined { get; set; }
    public List<Guid> WorldsCreated { get; set; }

    public User() : base()
    {
        Friends = new();
        WorldsJoined = new();
        WorldsCreated = new();
    }

    public User(string identityId, string username, string email, bool isOnline)
        : this()
    {
        IdentityId = identityId;
        Username = username;
        Email = email;
        IsOnline = isOnline;
    }
}