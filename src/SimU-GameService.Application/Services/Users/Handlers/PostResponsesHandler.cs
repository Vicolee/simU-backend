using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;

namespace SimU_GameService.Application.Services.Users.Commands;

public class PostResponsesHandler : IRequestHandler<PostResponsesCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public PostResponsesHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public Task<Unit> Handle(PostResponsesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}