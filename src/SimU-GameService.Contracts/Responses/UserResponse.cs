using SimU_GameService.Domain.Models;

namespace SimU_GameService.Contracts.Responses;

public record UserResponse(
    Guid Id,
    string Username,
    string? Email,
    string? Summary,
    Location? Location,
    DateTime CreatedTime,
    bool IsOnline,
    List<int> SpriteAnimations);