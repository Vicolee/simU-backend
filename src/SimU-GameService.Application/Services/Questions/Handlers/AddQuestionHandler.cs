using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Questions.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Questions.Handlers;

public class AddQuestionHandler : IRequestHandler<AddQuestionCommand, Unit>
{
    private readonly IQuestionRepository _questionRepository;

    public AddQuestionHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    /// <summary>
    /// Returns the AI generated summary for an agent based off the
    /// information users provided about it during its incubation process
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Unit> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question(request.QuestionText, request.Type) ?? throw new BadRequestException("Invalid question: Either did not provide a question and/or the question's type.");
        await _questionRepository.AddQuestion(question);

        return Unit.Value;
    }
}