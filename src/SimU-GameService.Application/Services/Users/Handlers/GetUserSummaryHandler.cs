using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetUserSummaryHandler : IRequestHandler<GetUserSummaryQuery, string?>
{
    private readonly IUserRepository _userRepository;

    public GetUserSummaryHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<string?> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(request.UserId)
            ?? throw new NotFoundException(nameof(User), request.UserId);
        return user.Summary;
    }
}
using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Queries;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetUserSummaryHandler : IRequestHandler<GetUserSummaryQuery, string?>
{
    private readonly IUserRepository _userRepository;

    public GetUserSummaryHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<string?> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
        => await _userRepository.GetUserSummary(request.UserId);
}