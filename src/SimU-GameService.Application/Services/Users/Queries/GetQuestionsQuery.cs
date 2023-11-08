using MediatR;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetQuestionsQuery() : IRequest<IEnumerable<string>>;