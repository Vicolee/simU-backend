namespace SimU_GameService.Contracts.Responses;

public record class RegisterUserResponse(
    string Username,
    string Email);