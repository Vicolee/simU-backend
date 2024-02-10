using MediatR;

namespace SimU_GameService.Application.Services.QuestionResponses.Commands;

public record PostResponseCommand(Guid TargetCharacterId,
    Guid ResponderId,
    Guid QuestionId,
    string Response) : IRequest<Unit>;