using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models;

public class Chat : Entity
{
    public Guid SenderID { get; private set; }
    public Guid ReceiverID { get; private set; }
    public string? Content { get; private set; }
    public bool IsGroupChat { get; private set; }
    public DateTime CreatedTime { get; private set; }

    public Chat() : base()
    {
    }

    public Chat(Guid senderID, Guid receiverID, string content, bool isGroupChat) : this()
    {
        SenderID = senderID;
        ReceiverID = receiverID;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.Now;
    }
}