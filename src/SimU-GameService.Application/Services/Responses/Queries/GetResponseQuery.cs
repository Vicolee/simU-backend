using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.QuestionResponses.Queries;

public record GetResponseQuery(Guid TargetCharacterId, Guid QuestionId) : IRequest<object?>;