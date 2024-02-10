using MediatR;

namespace SimU_GameService.Application.Services.QuestionResponses.Queries;

public record GetResponsesQuery(Guid TargetCharacterId) : IRequest<IEnumerable<object?>>;