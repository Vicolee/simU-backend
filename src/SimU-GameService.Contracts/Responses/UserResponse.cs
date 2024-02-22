using SimU_GameService.Domain.Models;

namespace SimU_GameService.Contracts.Responses;

public record UserResponse(
    Guid Id,
    string Username,
    string? Email,
    string? Summary,
    string? Description,
    Location? Location,
    DateTime CreatedTime,
    bool IsOnline,
    Uri? Sprite_URL,
    Uri? Sprite_Headshot_URL);