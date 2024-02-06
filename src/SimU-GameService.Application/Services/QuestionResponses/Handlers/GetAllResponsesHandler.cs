using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.QuestionResponses.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class GetAllResponsesHandler : IRequestHandler<GetAllResponsesQuery, IEnumerable<object?>>
{
    private readonly IQuestionResponseRepository _questionResponseRepository;

    public GetAllResponsesHandler(IQuestionResponseRepository questionResponseRepository)
    {
        _questionResponseRepository = questionResponseRepository;
    }

    /// <summary>
    /// Returns the AI generated summary for an agent based off the
    /// information users provided about it during its incubation process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<object?>> Handle(GetAllResponsesQuery request, CancellationToken cancellationToken)
    {
        return await _questionResponseRepository.GetAllResponses(request.TargetCharacterId);
    }
}