using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Questions.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Handlers;

public class GetIncubationQuestionsHandler : IRequestHandler<GetIncubationQuestionsQuery, IEnumerable<Question?>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetIncubationQuestionsHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    /// <summary>
    /// Returns the AI generated summary for an agent based off the
    /// information users provided about it during its incubation process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Question?>> Handle(GetIncubationQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await _questionRepository.GetIncubationQuestions();
    }
}