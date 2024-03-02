using MediatR;

namespace SimU_GameService.Application.Services.Questions.Commands;

public record PostQuestionCommand(string QuestionText, string Type) : IRequest<Unit>;