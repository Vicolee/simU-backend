using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Commands;

public record PostResponseCommand(Guid TargetCharacterId, Guid ResponderId, Guid QuestionId, string Response) : IRequest<Unit>;