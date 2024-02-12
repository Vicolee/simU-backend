namespace SimU_GameService.Contracts.Responses;

public record AgentResponse(
    Guid Id,
    string Username,
    string? Description,
    string? Summary,
    int X_coord,
    int Y_coord,
    Guid CreatorId,
    bool IsHatched,
    Uri? Sprite_URL,
    Uri? Sprite_Headshot_URL,
    DateTime CreatedTime,
    DateTime HatchTime
);