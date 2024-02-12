using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Queries;

public record GetResponsesToQuestionQuery(Guid TargetId, Guid QuestionId) : IRequest<IEnumerable<Response>>;