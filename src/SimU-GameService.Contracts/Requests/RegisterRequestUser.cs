namespace SimU_GameService.Contracts.Requests;

public record class RegisterRequestUser(
    string Username,
    string Password,
    string Email,
    bool IsAgent = false,
    string Description = "");