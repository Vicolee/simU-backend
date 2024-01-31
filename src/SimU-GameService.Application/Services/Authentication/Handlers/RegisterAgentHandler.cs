using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Authentication.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Authentication.Handlers;

public class RegisterAgentHandler : IRequestHandler<RegisterAgentCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterAgentHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<Guid> Handle(RegisterAgentCommand request, CancellationToken cancellationToken)
    {
        var agent = new User(
            string.Empty,
            request.Username,
            string.Empty,
            true,
            request.Description ?? string.Empty
        );

        await _userRepository.AddUser(agent);
        return agent.UserId;         
    }
}