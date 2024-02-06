using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Conversations.Queries;

public record GetConversationQuery(Guid ConversationId) : IRequest<Conversation?>;