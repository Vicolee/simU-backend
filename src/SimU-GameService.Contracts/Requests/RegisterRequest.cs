namespace SimU_GameService.Contracts.Requests;

public record RegisterRequest(
    string Username,
    string Email,
    string Password);