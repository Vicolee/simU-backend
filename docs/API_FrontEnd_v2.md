# Front-End API

## Table of Contents

- [Notes](#notes)
- [Authentication Endpoints](#authentication-endpoints)
- [World Endpoints](#world-endpoints)
- [User Endpoints](#user-endpoints)
- [Agent Endpoints](#agent-endpoints)
- [Chat Endpoints](#chat-endpoints)
- [Question Endpoints](#question-endpoints)
- [SignalR Endpoints](#signalr-endpoints)

## Notes

- This document will probably evolve (gradually). In case of connection errors, please refer to this document as the initial source of truth.
- We will use a combination of REST API and SignalR requests to fulfill the API contract. SignalR is solely used for instances where we might need to send/broadcast information to other clients in the game.
- We will not include the sender ID in SignalR requests, instead we will resolve the sender's identity using the connection ID to the server's SignalR hub.

## Authentication Endpoints

### RegisterUser

#### Description

Registers (and logs in) a new user with the game server.

#### Request

`POST /authentication/register`

```json
{
    "username": "string",
    "email": "string",
    "password": "string"
}
```

#### Response

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "authToken": "string"
}
```

### LoginUser

#### Description

Logs in an existing user with the game server.

#### Request

`POST /authentication/login`

```json
{
    "email" : "string",
    "password" : "string"
}
```

#### Response

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "authToken": "string"
}
```

### LogoutUser

#### Description

Logs out the user from the game.

#### Request

- `POST /authentication/logout/{id}`

#### Response

- `204 No Content`

## World Endpoints

### CreateWorld

#### Description

Creates a new world.

#### Request

`POST /worlds`

```json
{
    "creatorId": "00000000-0000-0000-0000-000000000000",
    "name": "string",
    "description": "string"
}
```

#### Response

```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "creatorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "description": "string",
    "worldCode": "string",
    "thumbnail_URL": "string"
}
```

### GetWorld

#### Description

Returns an object containing all the information regarding the world with given `id`.

#### Request

`GET /worlds/{id}`

#### Response

```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "creatorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "description": "string",
    "worldCode": "string",
    "thumbnail_URL": "string"
}
```

### GetWorldIdFromWorldCode

#### Description

Front-end provides a world's private world code, and the back-end returns the `worldId`, which the front-end can then use to call the "AddUserToWorld" method in the /worlds/ route.

#### Request

`GET /worlds/{code}`

#### Response

```json
{
    "id": "00000-00000-00000-00000-00000"
}
```

### GetWorldCreator

#### Description

Returns the `id` of the world's creator.

#### Request

`GET /worlds/{id}/creator`

#### Response

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "username": "string",
    "location": {
        "x_coord": "int",
        "y_coord": "int"
    },
    "isOnline": false,
    "isCreator": false,
    "sprite_URL": "string",
    "sprite_headshot_URL": "string"
}
```

### GetWorldUsers

#### Description

Returns a list of all users that are active in the specified world (both online and offline players). Typically called when a user joins the world.

#### Request

`GET /worlds/{id}/users`

#### Response

```json
[
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "username": "string",
        "location": {
            "x_coord": "int",
            "y_coord": "int"
        },
        "isOnline": false,
        "isCreator": false,
        "sprite_URL": "string",
        "sprite_headshot_URL": "string"
    },
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "username": "string",
        "location": {
            "x_coord": "int",
            "y_coord": "int"
        },
        "isOnline": true,
        "isCreator": true,
        "sprite_URL": "string",
        "sprite_headshot_URL": "string"
    }
]
```

### GetWorldAgents

#### Description

Returns a list of all agents (not offline players) that are currently on the specified world. Typically called when a user has just joined the server.

#### Request

`GET /worlds/{id}/agents`

#### Response

```json
[
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "username": "string",
        "description": "string",
        "summary": "string",
        "location": {
            "x_coord": "int",
            "y_coord": "int"
        },
        "isHatched": true,
        "hatchTime": "2024-01-01T00:01:00Z",
        "sprite_URL": "string",
        "sprite_headshot_URL" : "string"
    },
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "username": "string",
        "description": "string",
        "summary": "string",
        "location": {
            "x_coord": "int",
            "y_coord": "int"
        },
        "isHatched": true,
        "hatchTime": "2024-01-01T00:01:00Z",
        "sprite_URL": "string",
        "sprite_headshot_URL" : "string"
    }
]
```

### AddUserToWorld

#### Description

Adds a user to a worlds' list of users

#### Request

`POST /worlds/{id}/users/{userId}`

#### Response

`No Content`

### AddAgentToWorld

#### Description

Adds an agent to a worlds' list of agents

#### Request

`POST /worlds/{id}/agents/{agentId}`

#### Response

`No Content`

### DeleteWorld

#### Description

Deletes a world. Note that only the world's creator can delete it.

#### Request

`DELETE /worlds/{id}`

#### Response

`No Content`

### GetIncubatingAgents

#### Description

Returns a list of the IDs of currently incubating agents.

#### Request

`GET /worlds/{id}/agents/incubating`

#### Response

```json
{
    [
        {
            "id": "00000000-0000-0000-0000-000000000000",
            "hatchTime": "2024-01-01T00:01:00Z"
        },
        {
            "id": "00000000-0000-0000-0000-000000000000",
            "hatchTime": "2024-01-01T00:01:00Z"
        }
    ]
}
```

### GetHatchedAgents

#### Description

Returns a list of the IDs of hatched agents.

#### Request

`GET /worlds/{id}/agents/hatched`

#### Response

```json
[
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "hatchTime": "2024-01-01T00:01:00Z"
    },
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "hatchTime": "2024-01-01T00:01:00Z"
    }
]
```

### RemoveAgentFromWorld

#### Description

Removes an agent from a world. Only the owner of the world or the creator of the agent can remove it.

#### Request

`DELETE /worlds/{id}/agents/{agentId}`

#### Response

`No Content`

### RemoveUserFromWorld

#### Description

Kicks a player out of a world. Only the owner of the world can remove users. `userId` is the id of user being removed from the world.

#### Request

`DELETE /worlds/{id}/users/{userId}`

```json
{
    "ownerId": "00000000-0000-0000-0000-000000000000"
}
```

#### Response

`No Content`

## User Endpoints

### GetUser

#### Description

This endpoint returns the user object for the user with the given `id`.

#### Request

`GET /users/{id}`

#### Response

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "username": "string",
    "summary": "string",
    "email": "string",
    "location": {
        "x_coord": "int",
        "y_coord": "int"
    },
    "createdTime": "2024-01-01T00:01:00Z",
    "isOnline": true,
    "spriteAnimations": [1, 4, 2, 3]
}
```

### GetUserSummary

#### Description

Returns the GPT summary generated for a user.

#### Request

`GET /users/{userId}/summary`

#### Response

```json
{
    "summary": "string"
}
```

### GetUserWorlds

#### Description

Returns the list of worlds that a user belongs to.

#### Request

`GET /users/{id}/worlds`

#### Response

```json
[
    {
        "id": "00000000-0000-0000-0000-000000000000",
        "name": "string",
        "description": "string",
        "thumbnail_URL": "string"
    }
]
```

### UpdateUserSummary

#### Description

Updates a user's summary

#### Request

`PUT /users/{id}/summary`

#### Response

`200 OK`

### UpdateSprite

#### Description

Updates a user's sprite by providing a list of integers that refer to the different customizable parts of their character's sprite.

#### Request

`PUT /users/{id}/sprite`

```json
{
    "spriteAnimations": [1, 1, 0, 1]
}
```

#### Response

`200 OK`

## Agent Endpoints

### CreateAgent

#### Description

Creates an AI Agent.

#### Request

`POST /agents`

```json
{
    "username": "string",
    "description": "string",
    "creatorId": "00000000-0000-0000-0000-000000000000",
    "incubationDurationInHours": "int"
}
```

#### Response

```json
{
    "id": "00000000-0000-0000-0000-000000000000"
}
```

### GetAgent

#### Description

Returns information for the agent with the given `id`.

#### Request

`GET /agents/{id}`

#### Response

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "username": "string",
    "description": "string",
    "summary": "string",
    "location": {
        "x_coord": "int",
        "y_coord": "int"
    },
    "creatorId": "00000000-0000-0000-0000-000000000000",
    "isHatched": false,
    "sprite_URL": "string",
    "sprite_headshot_URL" : "string",
    "createdTime": "2023-11-04T22:54:19.911Z",
    "hatchTime": "2023-12-04T22:54:19.911Z",
}
```

### GetAgentSummary

#### Description

Grabs the GPT summary generated for an AI agent.

#### Request

`GET /agents/{agentId}/summary`

#### Response

```json
{
    "summary": "string"
}
```

### PostVisualDescription

#### Description

Posts an agent's visual description that is provided by the user and returns the URL to the updated agent sprite.

#### Request

`POST /agents/description`

```json
{
    "description" : "string"
}
```

#### Response

```json
{
    "sprite_URL" : "string",
    "sprite_headshot_URL" : "string"
}
```

## Chat Endpoints

### DeleteChat

#### Description

This endpoint allows a user to delete a message sent to a user/group through the server.

#### Request

`DELETE /chats/{chatId}`

#### Response

`No Content`

### GetChatHistory

#### Description

Gets the chat history between the current client and another user/group with `receiverId`.

#### Request

`GET /chats/history?participantA_Id={idA}&participantB_Id={idB}`

#### Response

```json
[
    {
        "id": "5fw92f53-5717-4562-b3fc-2c963f66afa6",
        "senderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "receiverId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "isGroupChat": true,
        "createdTime": "2023-11-04T23:07:50.727Z"
    },
    {
        "id": "5fw92f53-5717-4562-b3fc-2c963f66afa6",
        "senderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "receiverId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "isGroupChat": true,
        "createdTime": "2023-11-04T23:07:50.727Z"
    }
]
```

### GetUserChats

#### Description

Gets all the chats sent by the user matching given `id`.

#### Request

`GET /chats?senderId={id}`

#### Response

```json
[
    {
        "senderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "receiverId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "content": "string",
        "isGroupChat": true,
        "createdTime": "2023-11-04T23:10:06.106Z"
    }
]
```
### AskForQuestion

#### Description

When this route is pinged, the AI Service will be prompted by the back-end for a question from either an agent or offline user it is in charge of responding for. The question will be generated and then sent back to the front-end to the user (this is so agents can also initiate conversation).

#### Request

`GET /chats/question?senderId={idSender}&recipientId={idRecipient}`

#### Response

```json
[
    "question": "how are you doing today?"
]
```


## Question Endpoints

### GetUserQuestions

#### Description

Returns the questions a user responds to in order to create their own character when joining the game.

#### Request

`GET /questions/users`

#### Response

```json
[
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "question": "string"
    },
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "question": "string"
    }
]
```

### GetAgentQuestions

#### Description

Returns the questions used when training an agent.

#### Request

`GET /questions/agents`

#### Response

```json
[
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "question": "string"
    },
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "question": "string"
    }
]
```

### PostResponses

#### Description

**CALL THIS FOR WHEN UPLOADING USER RESPONSES** Records the users responses to the initial questionnaire or responses to questions about an incubating agent.

#### Request

`POST /questions/responses`

```json
{
    "targetId": "00000000-0000-0000-0000-000000000000",
    "responderId": "00000000-0000-0000-0000-000000000000",
    "responses": [
        {
            "questionId": "00000000-0000-0000-0000-000000000000",
            "response": "string"
        },
        {
            "questionId": "00000000-0000-0000-0000-000000000000",
            "response": "string"
        }
    ]
}
```

- `responderId` : ID of the user who is answering a question (either about themselves or an incubating agent).
- `targetId` : ID of the user or agent who is having questions answered about them. If a user is answering questions about themselves, the `responderId` and `targetId` is the same.

#### Response

`No Content`

### PostResponse

#### Description

**CALL THIS FOR WHEN UPLOADING AGENT RESPONSE BECAUSE YOU UPLOAD ONE AT A TIME FOR AGENT** Records the users responses to the initial questionnaire or responses to questions about an incubating agent.

#### Request

`POST /questions/response`

```json
{
    "targetId": "00000000-0000-0000-0000-000000000000",
    "responderId": "00000000-0000-0000-0000-000000000000",
    "questionId": "00000000-0000-0000-0000-000000000000",
    "response": "string"
}
```

- `responderId` : ID of the user who is answering a question (either about themselves or an incubating agent).
- `targetId` : ID of the user or agent who is having questions answered about them. If a user is answering questions about themselves, the `responderId` and `targetId` is the same.

#### Response

`No Content`

### GetResponses

#### Description

Returns all the questions and their corresponding responses about a specific user or agent.

#### Request

`GET /questions/responses/{id}`

#### Response

```json
[
    {
        "responderId": "00000000-0000-0000-0000-000000000000",
        "questionId": "00000000-0000-0000-0000-000000000000",
        "response": "string"
    },
    {
        "responderId": "00000000-0000-0000-0000-000000000000",
        "questionId": "00000000-0000-0000-0000-000000000000",
        "response": "string"
    }
]
```

- There can be multiple responses for one question.

### GetQuestionResponse

#### Description

Returns the responses for a specific question regarding a specific user or agent.

#### Request

`GET /questions/responses/{id}/{questionId}`

#### Response

```json
[
    {
        "responderId": "00000000-0000-0000-0000-000000000000",
        "response": "string"
    },
    {
        "responderId": "00000000-0000-0000-0000-000000000000",
        "response": "string"
    }
]
```

- Note: There can be multiple responses for one question.

## SignalR Endpoints

The SignalR endpoints are defined in the [`IUnityServer`](../src/SimU-GameService.Api/Hubs/Abstractions/IUnityServer.cs) and [`IUnityClient`](../src/SimU-GameService.Api/Hubs/Abstractions/IUnityClient.cs) interfaces.

- `IUnityServer` defines the contract for methods on the server hub that can be invoked by the client.
- `IUnityClient` defines the contract for methods (that should be) defined on the client that can be invoked by the server.

As an implementation reference, the `UpdateLocation` method is defined below.

### UpdateLocation

#### Description

Updates the user’s location in the map whenever a user changes location in the game.

#### Function prototype

```csharp
Task UpdateLocation(Location location);
```

#### Request

```csharp
HubConnection.SendAsync("UpdateLocation", new Location(0, 0))
```

#### Response

The `UpdateLocation` method on the server will broadcast the user’s new location to all other clients as shown below:

```csharp
await Clients.All.SendAsync("UpdateLocation", userId, location);
```

### SendChat

#### Description

Sends a message to a user/group through the server.

#### Function prototype

```csharp
Task SendChat(Guid receiverId, string message);
```

#### Request

```csharp
HubConnection.SendAsync("SendChat", Guid.NewGuid(), "Example message")
```

#### Response

The `MessageHandler` method on the server will forward the message to the `User` or `Group` matching the `receiverId`.
