using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
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

    public async Task<Unit> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question(request.QuestionText, request.Type);
        await _questionRepository.AddQuestion(question);
        return Unit.Value;
    }
}