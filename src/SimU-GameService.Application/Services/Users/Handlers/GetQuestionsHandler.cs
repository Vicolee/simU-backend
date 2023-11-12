using MediatR;
using SimU_GameService.Application.Common.Abstractions;

namespace SimU_GameService.Application.Services.Users.Queries;

public class GetQuestionsHandler : IRequestHandler<GetQuestionsQuery, IEnumerable<string>>
{
    private readonly IUserRepository _userRepository;

    public GetQuestionsHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<IEnumerable<string>> Handle(
        GetQuestionsQuery request,
        CancellationToken cancellationToken) => await _userRepository.GetQuestions();
}