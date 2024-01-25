namespace SimU_GameService.Infrastructure.Authentication;

public class AuthToken
{
    public string? Kind { get; set; }
    public string? LocalId { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string IdToken { get; set; } = default!;
    public bool? Registered { get; set; }
    public string? RefreshToken { get; set; }
    public string? ExpiresIn { get; set; }
}