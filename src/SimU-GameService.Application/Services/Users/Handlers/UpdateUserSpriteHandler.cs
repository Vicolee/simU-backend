using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateUserSpriteHandler : IRequestHandler<UpdateUserSpriteCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserSpriteHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateUserSpriteCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateUserSprite(request.UserId, request.SpriteURL, request.SpriteHeadshotURL);
        return Unit.Value;
    }
}