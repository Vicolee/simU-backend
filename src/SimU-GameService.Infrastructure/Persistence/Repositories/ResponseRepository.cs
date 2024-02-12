using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class ResponseRepository : IResponseRepository
{
    private readonly SimUDbContext _dbContext;

    public ResponseRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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

    public async Task<IEnumerable<object?>> GetResponses(Guid targetCharacterId)
    {
        return await _dbContext.QuestionResponses
            .Where(q => q.TargetId == targetCharacterId)
            .Select(q => new { q.TargetId, q.ResponderId, q.QuestionId, q.Content } )
            .ToListAsync();
    }

    public async Task<object?> GetResponse(Guid targetCharacterId, Guid questionId)
    {
        return await _dbContext.QuestionResponses
            .Where(q => q.TargetId == targetCharacterId && q.QuestionId == questionId)
            .Select(q => new { q.TargetId, q.ResponderId, q.QuestionId, q.Content } )
            .ToListAsync();
    }

}