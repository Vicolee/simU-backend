namespace SimU_GameService.Domain.Models;

using SimU_GameService.Domain.Primitives;

public class Conversation : Entity
{
    public DateTime CreatedTime { get; set; }
    public DateTime LastMessageTime { get; set; }
    public List<Guid> Participants { get; set; }
    public bool IsGroupChat { get; set; }
    public bool IsConversationOver { get; set; } = false;

    public Conversation() : base()
    {
        Participants = new();
    }

    public Conversation(Guid participant_A, Guid participant_B, bool isGroupChat) : this()
    {
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.UtcNow;
        Participants.Add(participant_A);
        Participants.Add(participant_B);
    }
}