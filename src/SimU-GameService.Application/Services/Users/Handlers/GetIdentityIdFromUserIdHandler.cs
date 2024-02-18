using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Queries;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetIdentityIdFromUserIdHandler : IRequestHandler<GetIdentityIdFromUserIdQuery, string>
{
    private readonly IUserRepository _userRepository;

    public GetIdentityIdFromUserIdHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<string> Handle(GetIdentityIdFromUserIdQuery request, CancellationToken cancellationToken)
        => await _userRepository.GetIdentityIdFromUserId(request.UserId);
}