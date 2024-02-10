using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.QuestionResponses.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class GetResponseHandler : IRequestHandler<GetResponseQuery, object?>
{
    private readonly IResponseRepository _responseRepository;

    public GetResponseHandler(IResponseRepository responseRepository)
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
    public async Task<object?> Handle(GetResponseQuery request, CancellationToken cancellationToken)
    {
        return await _responseRepository.GetResponse(request.TargetCharacterId, request.QuestionId);
    }
}