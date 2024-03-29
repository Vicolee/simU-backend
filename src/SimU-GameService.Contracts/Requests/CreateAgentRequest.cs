namespace SimU_GameService.Contracts.Requests;

public record CreateAgentRequest(
    string Username,
    string Description,
    Guid CreatorId,
    float IncubationDurationInHours,
    Uri SpriteURL,
    Uri SpriteHeadshotURL);