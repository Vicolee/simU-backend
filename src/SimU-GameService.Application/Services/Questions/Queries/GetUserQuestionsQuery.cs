using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Queries;

public record GetUserQuestionsQuery() : IRequest<IEnumerable<object?>>;