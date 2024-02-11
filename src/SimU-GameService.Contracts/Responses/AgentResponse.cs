using SimU_GameService.Domain.Models;

namespace SimU_GameService.Contracts.Responses;

public record AgentResponse(
    Guid Id,
    string Username,
    string Description,
    string Summary,
    Location Location,
    Guid CreatorId,
    bool IsHatched,
    string Sprite_URL,
    string Sprite_Headshot_URL,
    DateTime CreatedTime,
    DateTime HatchTime
);