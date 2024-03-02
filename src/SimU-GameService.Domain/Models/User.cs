namespace SimU_GameService.Domain.Models;

public class User : Character
{
    public string? Email { get; set; }
    public bool IsOnline { get; set; } = true;
    public string IdentityId { get; set; } = default!;
    public Guid ActiveWorldId { get; set; } = default;
    public List<Friend> Friends { get; set; }
    public List<Guid> WorldsJoined { get; set; }
    public List<Guid> WorldsCreated { get; set; }
    public List<int> SpriteAnimations { get; set; }
    public List<UserQuestionResponse> QuestionResponses { get; set; }

    public User() : base()
    {
        Friends = new();
        WorldsJoined = new();
        WorldsCreated = new();
        SpriteAnimations = new();
        QuestionResponses = new();
    }

    public User(string identityId, string username, string email, bool isOnline = true)
        : base(username, string.Empty)
    {
        IdentityId = identityId;
        Username = username;
        Email = email;
        IsOnline = isOnline;
        Friends = new();
        WorldsJoined = new();
        WorldsCreated = new();
        QuestionResponses = new();
        SpriteAnimations = new List<int> { 0, 0, 0, 0};
    }

    public void Logout() => IsOnline = false;
}