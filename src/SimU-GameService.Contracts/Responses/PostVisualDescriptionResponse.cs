namespace SimU_GameService.Contracts.Responses;

public record PostVisualDescriptionResponse(
    Uri SpriteURL,
    Uri SpriteHeadshotURL
);