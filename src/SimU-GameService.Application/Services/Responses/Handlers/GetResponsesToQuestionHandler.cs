using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.QuestionResponses.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Handlers;

public class GetResponsesToQuestionHandler : IRequestHandler<GetResponsesToQuestionQuery, IEnumerable<Response>>
{
    private readonly IResponseRepository _responseRepository;

    public GetResponsesToQuestionHandler(IResponseRepository responseRepository) => _responseRepository = responseRepository;

    public Task<IEnumerable<Response>> Handle(GetResponsesToQuestionQuery request, CancellationToken cancellationToken)
        => _responseRepository.GetResponsesToQuestion(request.TargetId, request.QuestionId);
}