namespace SimU_GameService.Contracts.Responses;

public record WorldResponse(
    Guid Id,
    Guid CreatorId,
    string Name,
    string Description,
    string WorldCode,
    Uri? Thumbnail_URL);