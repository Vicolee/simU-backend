using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Commands;

public record SendChatCommand(Guid SenderId, Guid ReceiverId, string Content) : IRequest<Chat?>;