namespace SimU_GameService.Application.Common.Abstractions;

public interface IAuthenticationService
{
    Task<Guid> RegisterUser(string firstName, string lastName, string email, string password);
    Task<Guid> RegisterAgent(string firstName, string lastName, Boolean isAgent, string description);
    Task<Guid> LoginUser(string email, string password);
}