namespace SimU_GameService.Application.Abstractions.Services;

public interface IAuthenticationService
{
    Task<string> RegisterUser(string email, string password, CancellationToken cancellationToken);
    Task<string> LoginUser(string email, string password);
}