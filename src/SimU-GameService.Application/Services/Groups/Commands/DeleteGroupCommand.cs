using MediatR;

namespace SimU_GameService.Application.Services.Groups.Commands;

public record DeleteGroupCommand(Guid GroupId) : IRequest<Unit>;