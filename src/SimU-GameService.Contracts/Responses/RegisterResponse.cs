namespace SimU_GameService.Contracts.Responses;

public record class RegisterResponse(
    Guid UserId,
    string Message);