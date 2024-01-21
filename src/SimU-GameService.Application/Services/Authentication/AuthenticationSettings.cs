namespace SimU_GameService.Application.Common.Authentication;

// TODO: move to Infrastructure layer?

public class AuthenticationSettings
{
    public string? TokenUri { get; set; }
    public string? ValidIssuer { get; set; }
    public string? Audience { get; set; }
}