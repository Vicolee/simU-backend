namespace SimU_GameService.Domain.Models;

public class User : Character
{
    public string? Email { get; set; }
    public bool IsOnline { get; set; } = false;
    public string IdentityId { get; set; } = default!;
    public Guid ActiveWorldId { get; set; } = default;
    public List<Friend> Friends { get; set; }
    public List<Guid> WorldsJoined { get; set; }
    public List<Guid> WorldsCreated { get; set; }
    public List<int> SpriteAnimations { get; set; }

    public User() : base()
    {
        Friends = new();
        WorldsJoined = new();
        WorldsCreated = new();
        SpriteAnimations = new();
    }

    public User(string identityId, string username, string email, bool isOnline)
        : base(username, string.Empty)
    {
        IdentityId = identityId;
        Username = username;
        Email = email;
        IsOnline = isOnline;
        Friends = new();
        WorldsJoined = new();
        WorldsCreated = new();
        SpriteAnimations = new List<int> { 0, 0, 0, 0};
    }

    public void Logout() => IsOnline = false;
}