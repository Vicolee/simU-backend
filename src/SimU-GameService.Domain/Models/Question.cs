using SimU_GameService.Domain.Primitives;
namespace SimU_GameService.Domain.Models;


public class Question : Entity
{
    public string Content { get; set; } = default!;
    public QuestionType QuestionType { get; set; }


    public Question() : base()
    {
    }

    public Question(string questionText, QuestionType questionType): this()
    {
        Content = questionText;
        QuestionType = questionType;
    }
}

public enum QuestionType
{
    UserQuestion,
    AgentQuestion,
    UserOrAgentQuestion
}
