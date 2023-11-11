using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public UpdateLocationHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateLocation(request.UserId, request.XCoord, request.YCoord);
        return Unit.Value;
    }
}