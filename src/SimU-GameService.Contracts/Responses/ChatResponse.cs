namespace SimU_GameService.Contracts.Responses;

public record ChatResponse(
    Guid SenderId,
    Guid ReceiverId,
    string Content,
    bool IsGroupChat,
    DateTime CreatedTime);