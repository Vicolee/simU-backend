using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Abstractions.Repositories;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IResponseRepository
{
    /// <summary>
    /// Adds response to a question to the repository.
    /// </summary>
    /// <param name="isUser"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    public Task PostResponse(bool isUser, Response response);

    /// <summary>
    /// Adds responses to questions (in bulk) to the repository.
    /// </summary>
    /// <param name="responses"></param>
    /// <param name="responses"></param>
    /// <returns></returns>
    public Task PostResponses(bool isUser, IEnumerable<Response> responses);

    /// <summary>
    /// Returns all the questions and corresponding responses about a user or agent.
    /// </summary>
    /// <param name="isUser"></param>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public Task<IEnumerable<Response>> GetResponses(bool isUser, Guid targetId);

    /// <summary>
    /// Returns the responses for a specific question about a user or agent.
    /// </summary>
    /// <param name="isUser"></param>
    /// <param name="targetId"></param>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public Task<IEnumerable<Response>> GetResponsesToQuestion(bool isUser, Guid targetId, Guid questionId);

    /// <summary>
    /// Returns a mapping of question IDs to responses for a specific character.
    /// </summary>
    /// <param name="isUser"></param>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public Task<(IEnumerable<Guid>, IEnumerable<IEnumerable<string>>)> GetQuestionIdResponsesMapping(bool isUser, Guid targetId);
}