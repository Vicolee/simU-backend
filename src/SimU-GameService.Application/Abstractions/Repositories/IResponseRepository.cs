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
    /// <param name="response"></param>
    /// <returns></returns>
    public Task PostResponse(Response response);

    /// <summary>
    /// Adds responses to questions (in bulk) to the repository.
    /// </summary>
    /// <param name="responses"></param>
    /// <returns></returns>
    public Task PostResponses(IEnumerable<Response> responses);

    /// <summary>
    /// Returns all the questions and corresponding responses about a user or agent.
    /// </summary>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public Task<IEnumerable<Response>> GetResponses(Guid targetId);

    /// <summary>
    /// Returns the responses for a specific question about a user or agent.
    /// </summary>
    /// <param name="targetId"></param>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public Task<IEnumerable<Response>> GetResponsesToQuestion(Guid targetId, Guid questionId);

    /// <summary>
    /// Returns a mapping of question IDs to responses for a specific character.
    /// </summary>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public Task<(IEnumerable<Guid>, IEnumerable<IEnumerable<string>>)> GetQuestionIdResponsesMapping(Guid targetId);
}