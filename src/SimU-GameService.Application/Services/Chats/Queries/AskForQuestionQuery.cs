using MediatR;

namespace SimU_GameService.Application.Services.Chats.Queries;

public record AskForQuestionQuery(Guid SenderId, Guid RecipientId) : IRequest<string?>;