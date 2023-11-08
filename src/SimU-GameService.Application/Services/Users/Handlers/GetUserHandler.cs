using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetUserHandler : IRequestHandler<GetUserQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<User?> Handle(GetUserQuery request,
        CancellationToken cancellationToken) => await _userRepository.GetUserById(request.UserId);
}