namespace SimU_GameService.Domain.Models;

public class UserQuestionResponse : Response
{
    public UserQuestionResponse() : base()
    {
    }

    public UserQuestionResponse(Guid responderId, Guid targetId, Guid questionId, string response)
        : base(responderId, targetId, questionId, response)
    {
    }
}