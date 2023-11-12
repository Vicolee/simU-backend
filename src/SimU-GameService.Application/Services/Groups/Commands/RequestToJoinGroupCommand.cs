using MediatR;

namespace SimU_GameService.Application.Services.Groups.Commands;

public record RequestToJoinGroupCommand(Guid GroupId) : IRequest<Guid>;