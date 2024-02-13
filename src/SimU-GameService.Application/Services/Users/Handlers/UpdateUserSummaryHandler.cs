using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateUserSummaryHandler : IRequestHandler<UpdateUserSummaryCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserSummaryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(UpdateUserSummaryCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateUserSummary(request.UserId, request.Summary);
        return Unit.Value;
    }
}