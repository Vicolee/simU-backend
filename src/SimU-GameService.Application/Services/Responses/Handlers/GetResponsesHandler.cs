using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.QuestionResponses.Queries;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class GetResponsesHandler : IRequestHandler<GetResponsesQuery, IEnumerable<object?>>
{
    private readonly IResponseRepository _responseRepository;

    public GetResponsesHandler(IResponseRepository responseRepository)
    {
        _responseRepository = responseRepository;
    }

    /// <summary>
    /// Returns the AI generated summary for an agent based off the
    /// information users provided about it during its incubation process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<object?>> Handle(GetResponsesQuery request, CancellationToken cancellationToken)
    {
        return await _responseRepository.GetResponses(request.TargetCharacterId);
    }
}