using MediatR;

namespace SimU_GameService.Application.Services.Chats.Commands;

public record DeleteChatCommand(Guid ChatId) : IRequest<Unit>;
