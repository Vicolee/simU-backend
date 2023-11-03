namespace SimU_GameService.Contracts.Responses;
public record UserResponse(
    string FirstName,
    string LastName,
    string Email,
    int LastKnownX,
    int LastKnownY,
    bool IsLoggedIn,
    DateTime CreatedTime,
    DateTime LastActiveTime);