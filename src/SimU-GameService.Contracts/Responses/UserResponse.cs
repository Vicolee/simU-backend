namespace SimU_GameService.Contracts.Responses;

public record UserResponse(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Description,
    int LastKnownX,
    int LastKnownY,
    DateTime CreatedTime);