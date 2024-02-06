using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Commands;

public record AddQuestionCommand(string QuestionText, QuestionType Type) : IRequest<Unit>;