using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record PostDescriptionCommand(
    Guid AgentId, string Description) : IRequest<(Uri, Uri)>;