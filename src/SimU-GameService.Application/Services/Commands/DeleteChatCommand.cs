using MediatR;

namespace SimU_GameService.Application.Services.Commands;

public record DeleteChatCommand(Guid ChatId) : IRequest<Unit>;
