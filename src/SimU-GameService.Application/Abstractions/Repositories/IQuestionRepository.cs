using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IQuestionRepository
{
    /// <summary>
    /// Adds incubation questions to the repository
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
    public Task AddQuestion(Question question);

    /// <summary>
    /// Grabs the training questions for the player to answer about themselves when they are creating their own character.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Question>> GetUserQuestions();

    /// <summary>
    /// Grabs the incubation questions for the user to answer about an agent they are creating.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Question>> GetAgentQuestions();

    /// <summary>
    /// Gets a question from the repository by ID.
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public Task<string> GetQuestion(Guid questionId);
}