# Front-End API

## Table of Contents

- [Notes](#notes)
- [Authentication Endpoints](#authentication-endpoints)
- [World Endpoints](#world-endpoints)
- [User Endpoints](#user-endpoints)
- [Agent Endpoints](#agent-endpoints)
- [Chat Endpoints](#chat-endpoints)
- [Question Endpoints](#question-endpoints)

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

- `PUT /authentication/{id}/logout`

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
    "id": "00000000-0000-0000-0000-000000000000"
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
    "id": "00000000-0000-0000-0000-000000000000",
    "creatorId": "00000000-0000-0000-0000-000000000000",
    "name": "string",
    "description": "string",
    "thumbnail_URL" : "string"
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
    "id": "00000000-0000-0000-0000-000000000000"
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
        "sprite_headshot_URL" : "string"
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
        "sprite_headshot_URL" : "string"
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
        "sprite_URL": "string",
        "sprite_headshot_URL" : "string"
    }
]
```

### DeleteWorld

#### Description

Deletes a world. Not that only the world's creator can delete it.

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

### GetHatchedAgent

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

### RemoveUserFromWorld

#### Description

Kicks a player out of a world. Only the owner of the world can remove users.

#### Request

`DELETE /worlds/{id}/users/{userId}`

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
    "email": "string",
    "location": {
        "x_coord": "int",
        "y_coord": "int"
    },
    "createdTime": "2024-01-01T00:01:00Z",
    "isOnline": true,
    "isCreator": false,
    "sprite_URL": "string",
    "sprite_headshot_URL" : "string"
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
        "thumbnail_URL" : "string"
    }
]
```

### AddUserWorld

#### Description

Adds an existing world to the user's list of worlds. Use case: for display in the homepage.

#### Request

`POST /users/{id}/worlds/{worldId}`

#### Response

`No Content`

### RemoveUserWorld

#### Description

Removes a world from the list of worlds that a user belongs to.

#### Request

`DELETE /users/{id}/worlds/{worldId}`

#### Response

`No Content`

### UpdateSprite

#### Description

Updates a user's sprite by providing a text description or the URL to a photo that will be used to generate the new sprite. We use the `isURL` flag to determine if the user is providing a URL or a description.

#### Request

`POST /users/{id}/sprite`

```json
{
    "description" : "string",
    "isURL": false
}
```

#### Response

`No Content`

### UpdateLocation

#### Description

Updates the user’s location in the map whenever a user changes location in the game.

#### Function prototype

```csharp
Task UpdateLocation(Location location);
public record Location(int X_coord, int Y_coord);
```

#### Request

```csharp
Location location = new Location(0, 0);
HubConnection.SendAsync("UpdateLocation", location)
```

#### Response

The `UpdateLocation` callback on the server will broadcast the user’s new location to all other clients using the client-side `UpdateLocation` callback as shown below:

```csharp
await Clients.All.SendAsync("UpdateLocation", userId, location);
```

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

## Chat Endpoints

### SendChat

#### Description

Sends a message to a user/group through the server.

#### Function prototype

```csharp
Task SendChat(Guid receiverId, string message);
```

#### Request

```csharp
Guid receiverId = Guid.NewGuid();
string message = "Example message"
HubConnection.SendAsync("SendChat", receiverId, message)
```

#### Response

The `SendChat` callback on the server will forward the message to the `User` or `Group` matching the `receiverId`.

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

`GET /chats/history`

```json
{
    "senderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "recipientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

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
        "id": "int",
        "question": "string"
    },
    {
        "id": "int",
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
        "id": "int",
        "question": "string"
    },
    {
        "id": "int",
        "question": "string"
    }
]
```

### PostResponses

#### Description

Records the users responses to the initial questionnaire or responses to questions about an incubating agent.

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

## Additional endpoints/suggestions

### LogoutUser

#### Description

Allows the front-end to notify the backend that the user has exited the app, so that the back-end can change the users status from online to offline.

#### Request

`PUT /authentication/logout/{userId}`

#### Response

`No Content`

*Note*: `BroadcastUserLogOut` and `BroadcastUserLogin` below are not endpoints but rather service functions that will be implemented using SignalR.

### BroadcastUserLogOut

#### Description

Notify all users in the same world as user X that user X has logged off the server.

### BroadcastUserLogin

#### Description

Notify all users in the same world as user X that user X has logged into the server.
