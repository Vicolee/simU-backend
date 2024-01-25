namespace SimU_GameService.Application.Common.Abstractions;

public interface IAuthenticationService
{
    Task<string> RegisterUser(string email, string password, CancellationToken cancellationToken);
    Task<string> LoginUser(string email, string password);
}