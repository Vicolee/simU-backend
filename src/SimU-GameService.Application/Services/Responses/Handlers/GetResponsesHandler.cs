using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.QuestionResponses.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class GetResponsesHandler : IRequestHandler<GetResponsesQuery, IEnumerable<Response>>
{
    private readonly IResponseRepository _responseRepository;

    public GetResponsesHandler(IResponseRepository responseRepository) => _responseRepository = responseRepository;
    
    public async Task<IEnumerable<Response>> Handle(GetResponsesQuery request, CancellationToken cancellationToken)
        => await _responseRepository.GetResponses(request.TargetId);
}