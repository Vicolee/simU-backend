using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Conversations.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Conversations.Handlers;

public class GetAllConversationsHandler : IRequestHandler<GetAllConversationsQuery, IEnumerable<Guid>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetAllConversationsHandler(IConversationRepository conversationRepository) => _conversationRepository = conversationRepository;

    public Task<IEnumerable<Guid>> Handle(GetAllConversationsQuery request,
    CancellationToken cancellationToken) => _conversationRepository.GetAllConversations(request.SenderId, request.ReceiverId);
}
