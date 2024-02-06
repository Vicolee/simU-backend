using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class QuestionResponseRepository : IQuestionResponseRepository
{
    private readonly SimUDbContext _dbContext;

    public QuestionResponseRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task PostResponse(Response questionResponse)
    {
        await _dbContext.QuestionResponses.AddAsync(questionResponse);

        // var user = _dbContext.Users
        //     .FirstOrDefault(u => u.UserId == userId) ?? throw new NotFoundException(nameof(User), userId);

        // user.QuestionResponses = responses
        //     .Select((response, index) => new QuestionResponse(_questions.ElementAt(index), response))
        //     .ToList();

        await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<object?>> GetAllResponses(Guid targetCharacterId)
    {
        return await _dbContext.QuestionResponses
            .Where(q => q.TargetId == targetCharacterId)
            .Select(q => new { q.TargetId, q.ResponderId, q.QuestionId, q.Content } )
            .ToListAsync(); // Add ToListAsync() method call here
    }

    public async Task<object?> GetResponse(Guid targetCharacterId, Guid questionId)
    {
        return await _dbContext.QuestionResponses
            .Where(q => q.TargetId == targetCharacterId && q.QuestionId == questionId)
            .Select(q => new { q.TargetId, q.ResponderId, q.QuestionId, q.Content } )
            .ToListAsync();
    }

}