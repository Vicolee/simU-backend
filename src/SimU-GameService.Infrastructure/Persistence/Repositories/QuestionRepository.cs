using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace SimU_GameService.Infrastructure.Persistence.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly SimUDbContext _dbContext;

    public QuestionRepository(SimUDbContext dbContext) => _dbContext = dbContext;

    public async Task AddQuestion(Question question)
    {
        await _dbContext.Questions.AddAsync(question);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Question>> GetUserQuestions() => await _dbContext.Questions
        .Where(q => q.QuestionType == QuestionType.UserQuestion || q.QuestionType == QuestionType.UserOrAgentQuestion)
        .ToListAsync();

    public async Task<IEnumerable<Question>> GetAgentQuestions() => await _dbContext.Questions
        .Where(q => q.QuestionType == QuestionType.AgentQuestion || q.QuestionType == QuestionType.UserOrAgentQuestion)
        .ToListAsync();
}