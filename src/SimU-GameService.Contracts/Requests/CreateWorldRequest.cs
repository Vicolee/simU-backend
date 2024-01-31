namespace SimU_GameService.Contracts.Requests;

public record CreateWorldRequest(
    Guid CreatorId,
    string Name,
    string Description);