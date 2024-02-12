using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Queries;

public record GetResponsesQuery(Guid TargetId) : IRequest<IEnumerable<Response>>;