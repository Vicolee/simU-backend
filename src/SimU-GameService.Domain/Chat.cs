namespace SimU_GameService.Domain;

public class Chat
{
    public Guid Id { get; private set; }
    public Guid SenderID { get; private set; }
    public Guid ReceiverID { get; private set; }
    public string Content { get; private set; }
    public bool IsGroupChat { get; private set; }
    public DateTime CreatedTime { get; private set; }

    public Chat()
    {
    }

    public Chat(Guid senderID, Guid receiverID, string content, bool isGroupChat)
    {
        Id = Guid.NewGuid();
        SenderID = senderID;
        ReceiverID = receiverID;
        Content = content;
        IsGroupChat = isGroupChat;
        CreatedTime = DateTime.Now;
    }
}