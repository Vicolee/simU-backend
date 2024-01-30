using SimU_GameService.Domain.Models;

namespace SimU_GameService.Contracts.Responses;

public record WorldUserResponse(Guid Id,
    string Username,
    Location Location,
    bool IsOnline,
    bool IsCreator,
    Uri Sprite_URL,
    Uri Sprite_Headshot_URL);