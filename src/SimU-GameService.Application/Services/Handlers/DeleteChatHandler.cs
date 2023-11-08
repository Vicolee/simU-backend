using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Commands;

namespace SimU_GameService.Application.Services.Handlers;

public class DeleteChatHandler : IRequestHandler<DeleteChatCommand, Unit>
{
    private readonly IChatRepository _chatRepository;

    public DeleteChatHandler(IChatRepository chatRepository) => _chatRepository = chatRepository;

    public async Task<Unit> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        await _chatRepository.DeleteChat(request.ChatId);
        return Unit.Value;
    }
}