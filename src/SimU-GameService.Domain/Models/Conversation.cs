namespace SimU_GameService.Domain.Models;
using SimU_GameService.Domain.Primitives;

public class Conversation : Entity
{
    public List<Guid> Participants { get; set; }
    public bool IsGroupChat { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime LastMessageTime { get; set; }
    public bool IsConversationOver { get; set; }

    public Conversation() : base()
    {
        Participants = new();
    }

    public Conversation(Guid character1Id, Guid character2Id, bool isGroupChat, DateTime? createdTime = null) : this()
    {
        IsGroupChat = isGroupChat;
        CreatedTime = createdTime ?? DateTime.UtcNow;
        Participants.Add(character1Id);
        Participants.Add(character2Id);
        IsConversationOver = false;
    }
}