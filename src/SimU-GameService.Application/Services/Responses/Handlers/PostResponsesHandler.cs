using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Responses.Commands;

namespace SimU_GameService.Application.Services.Responses.Handlers;

public class PostResponsesHandler : IRequestHandler<PostResponsesCommand, Unit>
{
    private readonly IResponseRepository _responseRepository;

    public PostResponsesHandler(IResponseRepository responseRepository) => _responseRepository = responseRepository;

    public async Task<Unit> Handle(PostResponsesCommand request, CancellationToken cancellationToken)
    {
        await _responseRepository.PostResponses(request.Responses);
        return Unit.Value;
    }
}