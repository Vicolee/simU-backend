using SimU_GameService.Domain.Primitives;
namespace SimU_GameService.Domain.Models;


public class Question : Entity
{
    public int QuestionNum { get; set; } // might not need this, thought if we wanted to order the questions that are given on the frontend
    public string? QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    // User, Agent, or Both are possible answers for QuestionType


    public Question() : base() {}

    public Question(
        int questionNum,
        string questionText,
        QuestionType questionType
        ) : this()
    {
        QuestionNum = questionNum;
        QuestionText = questionText;
        QuestionType = questionType;
    }
}

public enum QuestionType
{
    User,
    Agent,
    Both
}
