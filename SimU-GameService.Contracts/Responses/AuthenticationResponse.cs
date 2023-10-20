namespace SimU_GameService.Contracts.Responses;

public record class AuthenticationResponse(
    string UserId,
    string ResponseString);