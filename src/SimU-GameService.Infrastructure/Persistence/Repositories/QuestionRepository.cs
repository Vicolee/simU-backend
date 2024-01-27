using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly SimUDbContext _dbContext;

    public QuestionRepository(SimUDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddQuestion(Question question)
    {
        await _dbContext.Questions.AddAsync(question);
        await _dbContext.SaveChangesAsync();
    }

     public async Task<IEnumerable<object?>> GetUserQuestions()
    {
        return await _dbContext.Questions
            .Where(q => q.QuestionType == QuestionType.User || q.QuestionType == QuestionType.Both)
            .Select(q => new { q.Id, q.QuestionText } )
            .ToListAsync();
    }

    public async Task<IEnumerable<object?>> GetAgentQuestions()
    {
        return await _dbContext.Questions
            .Where(q => q.QuestionType == QuestionType.Agent || q.QuestionType == QuestionType.Both)
            .Select(q => new { q.Id, q.QuestionText } )
            .ToListAsync();
    }

}