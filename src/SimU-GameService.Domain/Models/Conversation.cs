namespace SimU_GameService.Domain.Models;
using SimU_GameService.Domain.Primitives;

public class Conversation : Entity
{
    public List<Guid> Participants { get; set; }
    public bool IsGroupChat { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime EndTime { get; set; }

    public Conversation() : base()
    {
        Participants = new();
    }

    public Conversation(string content, bool isGroupChat, DateTime? createdTime = null) : this()
    {
        IsGroupChat = isGroupChat;
        CreatedTime = createdTime ?? DateTime.UtcNow;
    }
}