namespace SimU_GameService.Domain.Models;

public class AgentQuestionResponse : Response
{
    public AgentQuestionResponse() : base()
    {
    }

    public AgentQuestionResponse(Guid responderId, Guid targetId, Guid questionId, string response)
        : base(responderId, targetId, questionId, response)
    {
    }
}