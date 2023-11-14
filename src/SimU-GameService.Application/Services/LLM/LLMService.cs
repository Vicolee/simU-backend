using System.Net.Http.Json;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Domain.Models;


namespace SimU_GameService.Application.Services;

public class LLMService : ILLMService
{
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _httpClient;

    // private readonly IUnityHub _unityHub;

    private readonly IChatRepository _chatRepository;

    public LLMService(IUserRepository userRepository, HttpClient httpClient, IChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _httpClient = httpClient;
        // _unityHub = unityHub;
        _chatRepository = chatRepository;
    }
    public async Task<Chat> RelayUserChat(Guid msgId, string msg, Guid userId, Guid agentId)
    {
        if (await _userRepository.GetUser(userId) == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        if (await _userRepository.GetUser(agentId) == null)
        {
            throw new NotFoundException(nameof(User), agentId);
        }

        var sourceAgentID = userId;
        var targetAgentID = agentId;
        var prompt = msg;
        var msgID = msgId;

        // we create a response Id ahead of time for the agent LLM to use, and then send it to them.
        Guid responseID = Guid.NewGuid();

        var request = new
        {
           sourceAgentID,
           targetAgentID,
           prompt,
           msgID,
           responseID
        };

        // The route for this LLM API call is set in the Dependency Injection file
        var response = await _httpClient.PostAsJsonAsync("", request);
        Console.WriteLine("here is the response: {0}", response);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine("here is the response: {0}", content);

        // TO DO: CHECK THIS WHILE DEBUGGING WE MIGHT NOT WANT TO CHECK ACCORDING TO 200 OK STATUS!!!
        var statusCode = response.StatusCode;

        // Checking if the response status code is 200 OK
        if (statusCode != System.Net.HttpStatusCode.OK)
        {
            throw new ServiceErrorException(statusCode, $"Failed to relay msg: {msgId}, with content: {msg}, from user: {userId}, to agent: {agentId}");
        }
        else
        {

            var agentResponse = new Chat
            {
                Id = responseID,
                SenderId = agentId,
                RecipientId = userId,
                Content = content,
                IsGroupChat = false
            };

            return agentResponse;
        }
    }
}

// // Check: Should we assume that error checking has been performed prior to this
// public async Task<string> RelayUserChat(Guid msgId, string msg, Guid userId, Guid agentId)
// {
//     if (await _userRepository.GetUserById(userId) == null)
//     {
//         return $"User {userId} does not exist";
//     }

//     if (await _userRepository.GetUserById(agentId) == null)
//     {
//         return $"Agent {agentId} does not exist";
//     }

//     try
//     {
//         var request = new
//         {
//             msgId,
//             msg,
//             userId,
//             agentId
//         };

//         // The route for this LLM API call is set in the Dependency Injection file
//         var response = await _httpClient.PostAsJsonAsync("", request);

//         // TO DO: CHECK THIS WHILE DEBUGGING WE MIGHT NOT WANT TO CHECK ACCORDING TO 200 OK STATUS!!!
//         var statusCode = response.StatusCode;

//         // Checking if the response status code is 200 OK
//         if (statusCode != System.Net.HttpStatusCode.OK)
//         {
//              return $"Error code: {statusCode}. Failed to relay msg: {msgId}, with content: {msg}, from user: {userId}, to agent: {agentId}";
//         }
//         else
//         {
//             // TO DO: CHOOSE A RESPONSE FORMAT WITH LLM GUYS
//             var responseContent = await response.Content.ReadAsStringAsync();
//             return responseContent;
//         }
//     }
//     catch (Exception e)
//     {
//         return e.Message;
//     }
// }
// [HttpPost("llm/response")]
// public async Task<string> RelayAgentResponse(Guid msgId, string msg, Guid userId, Guid agentId)
// {
//     if (await _userRepository.GetUserById(userId) == null)
//     {
//         return $"User {userId} does not exist";
//     }

//     if (await _userRepository.GetUserById(agentId) == null)
//     {
//         return $"Agent {agentId} does not exist";
//     }
// }


    // else
    //     {
    //         // TO DO: store entire message in another string to save in database after
    //         using (Stream stream = await response.Content.ReadAsStreamAsync())
    //         using (StreamReader reader = new StreamReader(stream))
    //         {
    //             char[] buffer = new char[1024];
    //             int bytesRead;
    //             // message to store in the database
    //             // TO DO: GET CHAT ID FROM LLM SERVICE
    //             string entireMessage = "";

    //             while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
    //             {
    //                 string chunk = new string(buffer, 0, bytesRead);
    //                 //to do: append the chatid in front of every chunk
    //                 Console.WriteLine(chunk);
    //                 entireMessage += chunk;
    //                 // await _unityHub.SendChat(agentId, chunk);
    //             }
    //             // send the user a terminating character so they know that the stream is over
    //             // await _unityHub.SendChat(agentId, "\n");
    //             // TO DO: REVISE THIS!!
    //             var agentResponse = new Chat
    //             {
    //                 Id = agentChatResponseId,
    //                 SenderId = agentId,
    //                 RecipientId = userId,
    //                 Content = entireMessage,
    //                 IsGroupChat = false
    //             };
    //             await _chatRepository.AddChat(agentResponse);
    //             // TO DO: REVISE THIS RETURN STATEMENT
    //             return entireMessage;
    //         }
    //     }