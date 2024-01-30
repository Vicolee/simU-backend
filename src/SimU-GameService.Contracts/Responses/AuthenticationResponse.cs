namespace SimU_GameService.Contracts.Responses;

public record class AuthenticationResponse(Guid Id, string AuthToken);