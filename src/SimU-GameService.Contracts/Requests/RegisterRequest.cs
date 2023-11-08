namespace SimU_GameService.Contracts.Requests;

public record class RegisterRequest(
    string FirstName,
    string LastName,
    string Password="",
    string Email="",
    bool IsAgent = false,
    string Description = "");