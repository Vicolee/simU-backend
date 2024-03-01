namespace SimU_GameService.Domain.Models;

using SimU_GameService.Domain.Primitives;

public class Conversation : Entity
{
    public DateTime CreatedTime { get; set; }
    public DateTime LastMessageSentAt { get; set; }
    public Guid ParticipantA { get; set; }
    public Guid ParticipantB { get; set; }
    public bool IsGroupChat { get; set; }
    public bool IsConversationOver { get; set; } = false;

    public Conversation() : base()
    {
    }

    public Conversation(Guid participant_A, Guid participant_B, bool isGroupChat) : this()
    {
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.UtcNow;
        LastMessageSentAt = DateTime.UtcNow;
        ParticipantA = participant_A;
        ParticipantB = participant_B;
    }
}