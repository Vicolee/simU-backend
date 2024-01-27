using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IQuestionResponseRepository
{
    /// <summary>
    /// Adds response to a question to the repository.
    /// </summary>
    /// <param name="questionResponse"></param>
    /// <returns></returns>
    public Task PostResponse(QuestionResponse questionResponse);

    /// <summary>
    /// Returns all the questions and corresponding responses about a user or agent.
    /// </summary>
    /// <param name="targetCharacterId"></param>
    /// <returns></returns>
    public Task<IEnumerable<object?>> GetAllResponses(Guid targetCharacterId);

    /// <summary>
    /// Returns the responses for a specific question about a user or agent.
    /// </summary>
    /// <param name="targetCharacterId"></param>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public Task<object?> GetResponse(Guid targetCharacterId, Guid questionId);
}