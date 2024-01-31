namespace SimU_GameService.Contracts.Responses;

public record GetWorldResponse(
    Guid Id,
    Guid CreatorId,
    string Name,
    string Description);