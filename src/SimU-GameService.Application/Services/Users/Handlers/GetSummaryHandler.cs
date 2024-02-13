using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetSummaryHandler : IRequestHandler<GetSummaryQuery, object?>
{
    private readonly IUserRepository _userRepository;

    public GetSummaryHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<object?> Handle(GetSummaryQuery request,
        CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUser(request.UserId);
            if (user == null)
            {
                return null;
            } else {
                return new {
                    user.Id,
                    user.Summary
                };
            }
        }
}