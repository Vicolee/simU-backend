namespace SimU_GameService.Contracts.Requests;

public record RegisterRequest(
    string Username,
    string Password,
    string Email);