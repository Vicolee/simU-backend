using MediatR;

namespace SimU_GameService.Application.Services.Groups.Commands;

public record CreateGroupCommand(string Name, Guid OwnerId) : IRequest<Guid>;