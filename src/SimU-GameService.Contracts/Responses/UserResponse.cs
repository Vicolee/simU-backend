namespace SimU_GameService.Contracts.Responses;

public record UserResponse(
    string Username,
    string? Email,
    string? Description,
    int X_coord,
    int Y_coord,
    DateTime CreatedTime);