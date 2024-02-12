using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Questions.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Handlers;

public class GetAgentQuestionsHandler : IRequestHandler<GetAgentQuestionsQuery, IEnumerable<Question>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetAgentQuestionsHandler(IQuestionRepository questionRepository) => _questionRepository = questionRepository;

    public async Task<IEnumerable<Question>> Handle(GetAgentQuestionsQuery request, CancellationToken cancellationToken)
        => await _questionRepository.GetAgentQuestions();
}