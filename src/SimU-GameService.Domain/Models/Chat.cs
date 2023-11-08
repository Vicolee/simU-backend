using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Chat : Entity
{
    public Guid SenderId { get; private set; }
    public Guid RecipientId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public bool IsGroupChat { get; private set; }
    public DateTime CreatedTime { get; private set; }

    public Chat() : base()
    {
    }

    public Chat(Guid senderID, Guid receiverID, string content, bool isGroupChat) : this()
    {
        SenderId = senderID;
        RecipientId = receiverID;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.Now;
    }
}