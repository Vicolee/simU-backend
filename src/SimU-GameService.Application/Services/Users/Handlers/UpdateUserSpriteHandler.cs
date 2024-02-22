using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Abstractions.Services;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateUserSpriteHandler : IRequestHandler<UpdateUserSpriteCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly ILLMService _llmService;

    public UpdateUserSpriteHandler(IUserRepository userRepository, ILLMService llmService)
    {
        _userRepository = userRepository;
        _llmService = llmService;
    }

    public async Task<Unit> Handle(UpdateUserSpriteCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateUserSprite(request.UserId, request.Animations);
        return Unit.Value;
    }
}