namespace SimU_GameService.Contracts.Requests;

public record class RegisterRequest(
    string FirstName,
    string LastName,
    string? Password = default,
    string? Email = default,
    string? Description = default,
    bool IsAgent = false);