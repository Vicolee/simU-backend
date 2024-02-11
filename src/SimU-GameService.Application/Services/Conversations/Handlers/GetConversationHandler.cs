using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Conversations.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Conversations.Handlers;

public class GetConversationHandler : IRequestHandler<GetConversationQuery, Conversation?>
{
    private readonly IConversationRepository _conversationRepository;

    public GetConversationHandler(IConversationRepository conversationRepository) => _conversationRepository = conversationRepository;

    public Task<Conversation?> Handle(GetConversationQuery request,
    CancellationToken cancellationToken) => _conversationRepository.GetConversation(request.ConversationId);
}
