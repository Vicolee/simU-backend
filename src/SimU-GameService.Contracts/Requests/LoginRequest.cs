namespace SimU_GameService.Contracts.Requests;

public record LoginRequest(
    string Email,
    string Password);