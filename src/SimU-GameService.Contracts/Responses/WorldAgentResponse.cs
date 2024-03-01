using SimU_GameService.Domain.Models;

namespace SimU_GameService.Contracts.Responses;

public record WorldAgentResponse(
    Guid Id,
    string Username,
    string? Description,
    string? Summary,
    Location? Location,
    DateTime CreatedTime,
    bool IsHatched,
    DateTime HatchTime,
    Uri? Sprite_URL,
    Uri? Sprite_Headshot_URL
);