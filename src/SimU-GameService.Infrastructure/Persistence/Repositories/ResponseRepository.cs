using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class ResponseRepository : IResponseRepository
{
    private readonly SimUDbContext _dbContext;

    public ResponseRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task PostResponse(bool isUser, Response response)
    {
        if (isUser)
        {
            await _dbContext.UserQuestionResponses.AddAsync((UserQuestionResponse)response);
        }
        else
        {
            await _dbContext.AgentQuestionResponses.AddAsync((AgentQuestionResponse)response);
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task PostResponses(bool isUser, IEnumerable<Response> responses)
    {
        if (isUser)
        {
            await _dbContext.UserQuestionResponses.AddRangeAsync(responses.Cast<UserQuestionResponse>());
        }
        else
        {
            await _dbContext.AgentQuestionResponses.AddRangeAsync(responses.Cast<AgentQuestionResponse>());
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Response>> GetResponses(bool isUser, Guid targetId)
    {
        if (isUser)
        {
            return await _dbContext.UserQuestionResponses
                .Where(r => r.TargetId == targetId).ToListAsync();
        }
        return await _dbContext.AgentQuestionResponses
            .Where(r => r.TargetId == targetId).ToListAsync();
    }

    public async Task<IEnumerable<Response>> GetResponsesToQuestion(bool isUser, Guid targetId, Guid questionId)
    {
        if (isUser)
        {
            return await _dbContext.UserQuestionResponses
                .Where(r => r.TargetId == targetId && r.QuestionId == questionId)
                .ToListAsync();
        }
        return await _dbContext.AgentQuestionResponses
            .Where(r => r.TargetId == targetId && r.QuestionId == questionId)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Guid>, IEnumerable<IEnumerable<string>>)> GetQuestionIdResponsesMapping(bool isUser, Guid targetId)
    {
        var questionResponses = await GetResponses(isUser, targetId);

        Dictionary<Guid, IEnumerable<string>> questionResponseMapping = questionResponses
            .GroupBy(qr => qr.QuestionId)
            .Select(g => new { QuestionId = g.Key, Responses = g.Select(qr => qr.Content) })
            .ToDictionary(g => g.QuestionId, g => g.Responses);

        return (questionResponseMapping.Keys, questionResponseMapping.Values);
    }
}