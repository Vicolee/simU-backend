using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Common.Authentication;


namespace SimU_GameService.Application.Services;

public class LargeLangModel : ILargeLangModel
{
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _httpClient;

    private readonly IUnityHub _unityHub;

    private readonly IChatRepository _chatRepository;

    public LargeLangModel(IUserRepository userRepository, HttpClient httpClient, IUnityHub unityHub, IChatRepository chatRepository)
    {
        _userRepository = userRepository;
        _httpClient = httpClient;
        _unityHub = unityHub;
        _chatRepository = chatRepository;
    }

     // Check: Should we assume that error checking has been performed prior to this
    public async Task<string> RelayUserChat(Guid msgId, string msg, Guid userId, Guid agentId)
    {
        if (await _userRepository.GetUserById(userId) == null)
        {
            return $"User {userId} does not exist";
        }

        if (await _userRepository.GetUserById(agentId) == null)
        {
            return $"Agent {agentId} does not exist";
        }

        try
        {
            var request = new
            {
                msgId,
                msg,
                userId,
                agentId
            };

            // The route for this LLM API call is set in the Dependency Injection file
            var response = await _httpClient.PostAsJsonAsync("", request);

            // TO DO: CHECK THIS WHILE DEBUGGING WE MIGHT NOT WANT TO CHECK ACCORDING TO 200 OK STATUS!!!
            var statusCode = response.StatusCode;

            // Checking if the response status code is 200 OK
            if (statusCode != System.Net.HttpStatusCode.OK)
            {
                 return $"Error code: {statusCode}. Failed to relay msg: {msgId}, with content: {msg}, from user: {userId}, to agent: {agentId}";
            }
            else
            {
                // TO DO: store entire message in another string to save in database after
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(stream))
                {
                    char[] buffer = new char[1024];
                    int bytesRead;
                    // message to store in the database
                    // TO DO: GET CHAT ID FROM LLM SERVICE
                    string entireMessage = "";

                    try {
                        while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            string chunk = new string(buffer, 0, bytesRead);
                            Console.WriteLine(chunk);
                            entireMessage += chunk;
                            _unityHub.SendMessage(userId, agentId, chunk);
                        }
                        // send the user a terminating character so they know that the stream is over
                        _unityHub.SendMessage(userId, agentId, "\n");
                        // TO DO: REVISE THIS!!
                        _chatRepository.AddChat(new Chat(msgId, entireMessage, userId, agentId));
                        // TO DO: REVISE THIS RETURN STATEMENT
                        return entireMessage;
                    }
                    catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
                    {
                        Console.WriteLine($"Error reading response: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception e)
        {
            return e.Message;
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
}