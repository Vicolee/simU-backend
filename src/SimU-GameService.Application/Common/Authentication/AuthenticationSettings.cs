namespace SimU_GameService.Application.Common.Authentication;

public class AuthenticationSettings
{
    public string? TokenUri { get; set; }
    public string? ValidIssuer { get; set; }
    public string? Audience { get; set; }
}