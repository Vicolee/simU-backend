using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Conversations.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Handlers;

public class IsOnGoingConversationHandler : IRequestHandler<IsOnGoingConversationQuery, Guid?>
{
    private readonly IConversationRepository _conversationRepository;

    public IsOnGoingConversationHandler(IConversationRepository conversationRepository) => _conversationRepository = conversationRepository;

    public Task<Guid?> Handle(IsOnGoingConversationQuery request,
    CancellationToken cancellationToken) => _conversationRepository.IsConversationOnGoing(request.SenderId, request.ReceiverId);
}
