using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Conversations.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Conversations.Handlers;

public class StartConversationHandler : IRequestHandler<StartConversationCommand, Guid?>
{
    private readonly IConversationRepository _conversationRepository;

    public StartConversationHandler(IConversationRepository conversationRepository) => _conversationRepository = conversationRepository;

    public async Task<Guid?> Handle(StartConversationCommand request, CancellationToken cancellationToken)
    {
        return await _conversationRepository.StartConversation(request.SenderId, request.ReceiverId);
    }
}