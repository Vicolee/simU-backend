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
        Dictionary<string, Uri> spriteURLs = request.IsURL ?
            await _llmService.GenerateSprites(request.UserId, request.Description) :
            await _llmService.GenerateSprites(request.UserId, photo_URL: new Uri(request.Description));

        Uri spriteURL = spriteURLs["sprite_URL"];
        Uri spriteHeadshotURL = spriteURLs["sprite_headshot_URL"];

        await _userRepository.UpdateUserSprite(request.UserId, spriteURL, spriteHeadshotURL);
        return Unit.Value;
    }
}