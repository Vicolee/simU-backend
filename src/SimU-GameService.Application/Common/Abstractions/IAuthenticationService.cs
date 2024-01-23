namespace SimU_GameService.Application.Common.Abstractions;

public interface IAuthenticationService
{
    Task<Guid> RegisterUser(string username, string email, string password);
    Task<Guid> RegisterAgent(string username, Guid createdByUser, int collabDurationHours, Boolean isAgent, string description);
    Task<Guid> LoginUser(string email, string password);
}