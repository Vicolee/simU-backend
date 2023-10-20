namespace SimU_GameService.Contracts.Requests;

public record class LoginRequest(
    string Email,
    string Password);