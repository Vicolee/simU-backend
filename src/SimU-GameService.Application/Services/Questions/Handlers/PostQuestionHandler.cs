using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Questions.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Handlers;

public class PostQuestionHandler : IRequestHandler<PostQuestionCommand, Unit>
{
    private readonly IQuestionRepository _questionRepository;

    public PostQuestionHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Unit> Handle(PostQuestionCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<QuestionType>(request.Type, out var questionType))
        {
            throw new BadRequestException("Invalid question type");
        }
        if (string.IsNullOrWhiteSpace(request.QuestionText))
        {
            throw new BadRequestException("Question text cannot be empty");
        }

        var question = new Question(request.QuestionText, questionType);
        await _questionRepository.AddQuestion(question);
        return Unit.Value;
    }
}