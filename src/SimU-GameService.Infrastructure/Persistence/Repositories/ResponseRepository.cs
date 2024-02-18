using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class ResponseRepository : IResponseRepository
{
    private readonly SimUDbContext _dbContext;

    public ResponseRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task PostResponse(Response response)
    {
        await _dbContext.QuestionResponses.AddAsync(response);
        await _dbContext.SaveChangesAsync();
    }

    public async Task PostResponses(IEnumerable<Response> responses)
    {
        await _dbContext.QuestionResponses.AddRangeAsync(responses);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Response>> GetResponses(Guid targetId) => await _dbContext.QuestionResponses
        .Where(r => r.TargetId == targetId)
        .ToListAsync();

    public async Task<IEnumerable<Response>> GetResponsesToQuestion(Guid targetId, Guid questionId) => await _dbContext.QuestionResponses
        .Where(r => r.TargetId == targetId && r.QuestionId == questionId)
        .ToListAsync();

    public async Task<(IEnumerable<Guid>, IEnumerable<IEnumerable<string>>)> GetQuestionIdResponsesMapping(Guid targetId)
    {
        var questionResponses = await GetResponses(targetId);

        Dictionary<Guid, IEnumerable<string>> questionResponseMapping = questionResponses
            .GroupBy(qr => qr.QuestionId)
            .Select(g => new { QuestionId = g.Key, Responses = g.Select(qr => qr.Content) })
            .ToDictionary(g => g.QuestionId, g => g.Responses);

        return (questionResponseMapping.Keys, questionResponseMapping.Values);
    }
}