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

    public Chat() : base()
    {
    }

    public Chat(Guid senderId, Guid receiverId, Guid conversationId, string content, bool isGroupChat, Guid? id = null, DateTime? createdTime = null) : this()
    {
        SenderId = senderId;
        RecipientId = receiverId;
        ConversationId = ConversationId;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = createdTime ?? DateTime.UtcNow;
    }
}