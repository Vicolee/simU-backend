using SimU_GameService.Application.Services.Responses.Commands;
using SimU_GameService.Contracts.Requests;
using SimU_GameService.Contracts.Responses;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Api.Common;

public class Mapper : IMapper
{
    public WorldResponse MapToWorldResponse(World world) => new
        (
            world.Id,
            world.CreatorId,
            world.Name,
            world.Description,
            world.WorldCode,
            world.ThumbnailURL
        );

    public UserResponse MapToUserResponse(User user) => new
        (
            user.Id,
            user.Username,
            user.Email,
            user.Summary,
            user.Location,
            user.CreatedTime,
            user.IsOnline,
            user.SpriteAnimations
        );

    public AgentResponse MapToAgentResponse(Agent agent) => new
        (
            agent.Id,
            agent.Username,
            agent.Description,
            agent.Summary,
            agent.Location,
            agent.CreatorId,
            agent.IsHatched,
            agent.SpriteURL,
            agent.SpriteHeadshotURL,
            agent.CreatedTime,
            agent.HatchTime
        );

    public IncubatingAgentResponse MapToIncubatingAgentResponse(Agent agent)
        => new(agent.Id, agent.HatchTime);

    public QuestionResponse MapToQuestionResponse(Question question)
        => new(question.Id, question.Content);

    public PostResponsesCommand MapToPostResponsesCommand(ResponsesRequest request)
    {
        var responses = request.Responses
            .Select(r => new Response(request.ResponderId, request.TargetId, r.QuestionId, r.Response));
        return new PostResponsesCommand(request.TargetId, responses);
    }

    public AnswersResponse MapToAnswersResponse(Response response) => new
        (
            response.QuestionId,
            response.ResponderId,
            response.Content
        );

    public AnswersToQuestionResponse MapToAnswersToQuestionResponse(Response response) => new
        (
            response.ResponderId,
            response.Content
        );

    public WorldAgentResponse MapToWorldAgentResponse(Agent agent) => new
        (
            agent.Id,
            agent.Username,
            agent.Description,
            agent.Summary,
            agent.Location,
            agent.IsHatched,
            agent.HatchTime,
            agent.SpriteURL,
            agent.SpriteHeadshotURL
        );

    public ChatResponse MapToChatResponse(Chat chat) => new
    (
        chat.Id,
        chat.SenderId,
        chat.RecipientId,
        chat.Content,
        chat.IsGroupChat,
        chat.CreatedTime
    );
}

public interface IMapper
{
    WorldResponse MapToWorldResponse(World world);
    UserResponse MapToUserResponse(User creator);
    AgentResponse MapToAgentResponse(Agent agent);
    WorldAgentResponse MapToWorldAgentResponse(Agent agent);
    IncubatingAgentResponse MapToIncubatingAgentResponse(Agent agent);
    QuestionResponse MapToQuestionResponse(Question question);
    PostResponsesCommand MapToPostResponsesCommand(ResponsesRequest request);
    AnswersResponse MapToAnswersResponse(Response response);
    AnswersToQuestionResponse MapToAnswersToQuestionResponse(Response response);
    ChatResponse MapToChatResponse(Chat chat);
}