namespace SimU_GameService.Domain.Models;

public class Chat
{
    public Guid Id {get; set;}
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsGroupChat { get; set; }
    public DateTime CreatedTime { get; set; }

    public Chat()
    {
    }

    public Chat(Guid senderId, Guid receiverId, string content, bool isGroupChat, Guid? id = null, DateTime? createdTime = null) : this()
    {
        Id = id ?? Guid.NewGuid();
        SenderId = senderId;
        RecipientId = receiverId;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = createdTime ?? DateTime.UtcNow;
    }
}