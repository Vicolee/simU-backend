using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class RemoveWorldFromListHandler : IRequestHandler<RemoveWorldFromListCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public RemoveWorldFromListHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(RemoveWorldFromListCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.RemoveWorldFromList(request.UserId, request.WorldId);
        return Unit.Value;
    }
}