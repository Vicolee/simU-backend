using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class User : Character
{
    public Guid IdentityId { get; set; }
    public string? Email { get; set; }
    public bool IsAgent { get; set; } = false; // if we delete this IsAgent boolean, make sure to update code in SendChatHandler.cs
    public bool IsOnline { get; set; } = false;
    public List<QuestionResponse> QuestionResponses { get; set; }
    public List<Guid> WorldsJoined { get; set; }
    public List<Guid> WorldsCreated { get; set; }

    public User() : base()
    {
        QuestionResponses = new();
        WorldsJoined = new();
        WorldsCreated = new();
    }

    public User(
        Guid identityId,
        string username,
        string email,
        bool isAgent,
        bool isOnline
        ) : this()
    {
        IdentityId = identityId;
        Username = username;
        Email = email;
        IsAgent = isAgent;
        IsOnline = isOnline;
    }
}