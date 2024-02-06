using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Conversations.Queries;

public record IsOnGoingConversationQuery(Guid SenderId, Guid ReceiverId) : IRequest<Guid?>;