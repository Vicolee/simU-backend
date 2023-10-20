namespace SimU_GameService.Contracts.Requests;

public record class RegisterRequest(
    string Username,
    string Password,
    string Email);