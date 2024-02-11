namespace SimU_GameService.Domain.Models;
using SimU_GameService.Domain.Primitives;

public class Chat : Entity
{
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsGroupChat { get; set; }
    public DateTime CreatedTime { get; set; }

    // WasSenderOnline: records whether a user was online or offline
    // when they sent a message. If the sender is an agent, the value
    // will always be set to false.
    public bool WasSenderOnline { get; set; }

    public Chat() : base()
    {
    }

    public Chat(Guid senderId, Guid receiverId, Guid conversationId, string content, bool wasSenderOnline, bool isGroupChat, Guid? id = null, DateTime? createdTime = null) : this()
    {
        SenderId = senderId;
        RecipientId = receiverId;
        ConversationId = ConversationId;
        Content = content;
        WasSenderOnline = wasSenderOnline;
        IsGroupChat = isGroupChat;
        CreatedTime = createdTime ?? DateTime.UtcNow;
    }
}