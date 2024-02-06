using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Queries;

public record GetAllResponsesQuery(Guid TargetCharacterId) : IRequest<IEnumerable<object?>>;