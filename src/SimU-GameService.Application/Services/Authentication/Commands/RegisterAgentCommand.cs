using MediatR;

namespace SimU_GameService.Application.Services.Authentication.Commands;

public record RegisterAgentCommand(
    string FirstName, string LastName, string? Description) : IRequest<Guid>;
