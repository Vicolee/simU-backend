using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a user to the repository.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task AddUser(User user);

    /// <summary>
    /// Removes a user from the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task RemoveUser(Guid userId);

    /// <summary>
    /// Gets a user from the repository by ID.
    /// </summary>
    /// <param name="userId"</param>
    /// <returns></returns>
    public Task<User?> GetUserById(Guid userId);

    /// <summary>
    /// Gets a user from the repository by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<User?> GetUserByEmail(string email);

    /// <summary>
    /// Gets the entrance questionnaire prompts from the repository.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<string>> GetQuestions();

    /// <summary>
    /// Posts the entrance questionnaire responses for a user to the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="responses"></param>
    /// <returns></returns>
    Task PostResponses(Guid userId, IEnumerable<string> responses);

    /// <summary>
    /// Removes a friend from a user's friend list in the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendId"></param>
    /// <returns></returns>
    Task RemoveFriend(Guid userId, Guid friendId);
}
