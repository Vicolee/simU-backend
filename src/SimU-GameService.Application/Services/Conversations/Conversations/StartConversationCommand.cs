using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Conversations.Commands;

public record StartConversationCommand(Guid SenderId, Guid ReceiverId, bool IsGroupChat = false) : IRequest<Guid?>;
