using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Questions.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Handlers;

public class GetUserQuestionsHandler : IRequestHandler<GetUserQuestionsQuery, IEnumerable<Question>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetUserQuestionsHandler(IQuestionRepository questionRepository) => _questionRepository = questionRepository;

    public async Task<IEnumerable<Question>> Handle(GetUserQuestionsQuery request, CancellationToken cancellationToken)
        => await _questionRepository.GetUserQuestions();
}