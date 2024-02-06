namespace SimU_GameService.Contracts.Requests;

public record class RegisterRequestAgent(
    string Username,
    Guid CreatedByUser,
    int CollabDurationHours,
    string Description = "");