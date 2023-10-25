using Microsoft.AspNetCore.Components.Routing;

namespace GameService.Models;
public class Chat {
    public string? chatID { get; set; }
    public string? senderID { get; set; }
    public string? receiverID { get; set; } // can be a group or user id
    public Boolean isGroup { get; set; }
    public string? content { get; set; }
    public DateTime createdAt { get; set; }

}
