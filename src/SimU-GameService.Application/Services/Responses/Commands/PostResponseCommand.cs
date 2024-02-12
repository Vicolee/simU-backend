using MediatR;

namespace SimU_GameService.Application.Services.Responses.Commands;

public record PostResponseCommand(Guid TargetCharacterId,
    Guid ResponderId,
    Guid QuestionId,
    string Response) : IRequest<Unit>;