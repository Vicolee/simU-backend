using MediatR;

namespace SimU_GameService.Application.Services.Authentication.Commands;

public record RegisterAgentCommand(
    string Username, string? Description) : IRequest<Guid>;
