using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Common.Abstractions;

/// <summary>
/// This interface is used to abstract the database from services in the Application layer.
/// We define the methods that we want to use in the Application layer here.
/// These methods are implemented in the Infrastructure layer.
/// </summary>
public interface IAgentRepository
{
    /// <summary>
    /// Adds an agent to the repository.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public Task AddAgent(Agent agent);

    /// <summary>
    /// Removes a agent from the repository.
    /// </summary>
    /// <param name="agentId"></param>
    /// <returns></returns>
    public Task RemoveAgent(Guid agentId);

    /// <summary>
    /// Gets an agent from the repository by ID.
    /// </summary>
    /// <param name="agentId"</param>
    /// <returns></returns>
    public Task<Agent?> GetAgent(Guid agentId);

    /// <summary>
    /// Gets an agent's summary from the repository by ID.
    /// </summary>
    /// <param name="agentId"</param>
    /// <returns></returns>
    public Task<string?> GetSummary(Guid agentId);

    /// <summary>
    /// Gets the entrance questionnaire prompts from the repository.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<string>> GetQuestions();

    /// <summary>
    /// Posts the entrance questionnaire responses for a user to the repository.
    /// </summary>
    /// <param name="agentId"></param>
    /// <param name="responses"></param>
    /// <returns></returns>
    Task PostResponses(Guid userId, IEnumerable<string> responses);

    /// <summary>
    /// Updates a user's location in the repository.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="xCoord"></param>
    /// <param name="yCoord"></param>
    /// <returns></returns>
    Task UpdateLocation(Guid agentId, int xCoord, int yCoord);
    Task<Location?> GetLocation(Guid locationId);
}
