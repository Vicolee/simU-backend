namespace SimU_GameService.Domain.Models;

using SimU_GameService.Domain.Primitives;

public class Chat : Entity
{
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = default!;
    public bool IsGroupChat { get; set; }
    public DateTime CreatedTime { get; set; }

    public Chat() : base()
    {
    }

    public Chat(Guid senderId, Guid receiverId, Guid conversationId, string content, bool isGroupChat) : this()
    {
        SenderId = senderId;
        RecipientId = receiverId;
        ConversationId = conversationId;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.UtcNow;
    }
}