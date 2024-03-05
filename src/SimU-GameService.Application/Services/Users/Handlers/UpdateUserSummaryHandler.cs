using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateUserSummaryHandler : IRequestHandler<UpdateUserSummaryCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly ILLMService _llmService;

    public UpdateUserSummaryHandler(IUserRepository userRepository, ILLMService llmService)
    {
        _userRepository = userRepository;
        _llmService = llmService;
    }

    public async Task<Unit> Handle(UpdateUserSummaryCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateUserSummary(request.UserId, request.Summary);
        await _llmService.UpdateUserSummary(request.UserId, request.Summary);
        return Unit.Value;
    }
}