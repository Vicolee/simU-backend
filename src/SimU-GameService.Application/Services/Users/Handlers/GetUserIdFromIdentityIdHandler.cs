using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetUserIdFromIdentityIdHandler : IRequestHandler<GetUserIdFromIdentityIdQuery, Guid>
{
    private readonly IUserRepository _userRepository;

    public GetUserIdFromIdentityIdHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<Guid> Handle(GetUserIdFromIdentityIdQuery request, CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetUserIdFromIdentityId(request.IdentityId);
        if (userId == default)
        {
            throw new NotFoundException(nameof(User), request.IdentityId);
        }
        return userId;
    }
}