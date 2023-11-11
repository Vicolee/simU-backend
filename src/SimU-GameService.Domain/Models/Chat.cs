using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Chat
{
    public Guid Id {get; set;}
    public Guid SenderID { get; set; }
    public Guid ReceiverID { get; set; }
    public string? Content { get; set; }
    public bool IsGroupChat { get; set; }
    public DateTime CreatedTime { get; set; }

    public Chat()
    {
    }

    public Chat(Guid senderID, Guid receiverID, string content, bool isGroupChat, Guid? id = null) : this()
    {
        Id = id ?? Guid.NewGuid();
        SenderID = senderID;
        ReceiverID = receiverID;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.Now;
    }
}