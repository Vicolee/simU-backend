namespace SimU_GameService.Contracts.Responses;

public record ChatResponse(
    Guid Id,
    Guid SenderId,
    Guid ReceiverId,
    string Content,
    bool IsSenderOnline,
    DateTime CreatedTime);