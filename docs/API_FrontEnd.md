# Front-End to GameService API

## Table of Contents

- [Notes](#notes)
- [Authentication Endpoints](#authentication-endpoints)
- [World Endpoints](#world-endpoints)
- [User Endpoints](#user-endpoints)
- [Agent Endpoints](#agent-endpoints)
- [Group Endpoints](#group-endpoints)
- [Chat Endpoints](#chat-endpoints)
- [Question Endpoints](#question-endpoints)

## Notes

- We will use a combination of REST API and SignalR requests to fulfill the API contract. SignalR is solely used for instances where we might need to send/broadcast information to other clients in the game.
- You might notice that the SignalR requests do not include the sender's ID. We will resolve the sender's identity using the connection ID to the server's SignalR hub.
- This document will probably evolve (gradually). In case of connection errors, please refer to this document as the initial source of truth.

## Authentication Endpoints

### RegisterUser

#### Description

- This endpoint registers (and logs in) a user with the game server.

#### Request

- `POST /authentication/register/user`

  ```json
  {
    "username": "string",
    "password": "string",
    "email": "string"
  }
  ```

#### Response

- `200 OK`

  ```json
  {
    "id": "string",
    "authToken": "string"
  }
  ```

### LoginUser

#### Description

- This endpoint logs in a user with the game server.

#### Request

- `POST /authentication/login`

  ```json
  {
    "email" : "string",
    "password" : "string"
  }
  ```

#### Response

- `200 OK`

  ```json
  {
    "id": "string",
    "authToken": "string"
  }
  ```

### LogoutUser - IMPORTANT: NEED AN ENDPOINT FOR THE FRONTEND TO NOTIFY US WHEN A USER LOGS OUT OF AN APPLICATION

#### Description

Endpoint for the front-end to notify the backend that the user has closed the app, so that the back-end can change the users status from online to offline.

#### Request

`PUT /authentication/{userId}/logout`

#### Response

- `204 No Content`

## World Endpoints

### CreateWorld

#### Description

Creates a new world for the user.

#### Request

- `POST /worlds/create`

```json
  {
    "worldName": "string",
    "userCreatorId": "user Id as string",
    "description": "string"
  }
  ```

#### Response

- `200 OK`

 ```json
  {
    "id": "string",
    "privateCode": "string (5 digits that we randomly generate - use while loop to check that the 5 digits haven't been used for another world.)"
  }
  ```

### AddWorldToList

#### Description

Adds another user's preexisting world to your "Worlds homepage screen" so that you can later connect to it

#### Request

- `GET /worlds/add/{privateCode}`

```json
  {
    "privateCode": "5XW2R" (5 characters),
    "userId": "user Id as string"
  }
  ```

#### Response

- `200 OK`

 ```json
  {
    "worldId": "string",
    "worldName": "string",
    "description": "string"
  }
  ```

#### Response

- `200 OK`

### GetWorldInfo (call this simultaneously with InitializeUserGameObjects and InitializeAgentGameObjects)

#### Description

#### Request

- `GET /worlds/{worldId}`

#### Response

- `200 OK`

```json
  {
    "worldId": "string",
    "worldName": "string",
    "ownerId": "string",
  }
  ```

### InitializeUserGameObjects() (TO LEKINA: THIS SHOULD BE SIGNALR)

#### Description

This endpoint returns a list of all users that belong to the server, along with their online/offline status. Typically, this function will be called when a user has just joined the server.

#### Request

- `GET /worlds/{worldId}/users`

#### Response

- `200 OK`

```json
{
  "userId1": { "locationX": 22, "locationY": 24, "characterSprite": "null", "isOnline": "True" },
  "userId2": { "locationX": 50, "locationY": 63, "characterSprite": "null", "isOnline": "False" }
}
```

### InitializeAgentGameObjects() (TO LEKINA: THIS SHOULD BE SIGNALR)

#### Description

This endpoint returns a list of all agents that are currently on the server. Typically, this function will be called when a user has just joined the server. **Note to Lekina: We must be careful about whether to include offline players whose AI trained personalities are now playing on the server**

#### Request

- `GET /worlds/{worldId}/agents`

#### Response

- `200 OK`

```json
{
  "agentId1": { "locationX": 33, "locationY": 4, "characterSprite: null" },
  "agentId2": { "locationX": 21, "locationY": 28, "characterSprite: null" }
}
```

### DeleteWorld

#### Description

Deletes a user's world if they are the creator of it.

#### Request

- `DELETE /worlds/remove/{worldId}`

```json
  {
    "worldId": "string",
    "userId": "user Id as string",
    "userPass": "string"
  }
  ```

#### Response

- `200 OK`

### GetUserList

#### Description

This endpoint returns agent information for the Players list screen.

#### Request

- `GET /worlds/{worldId}/users/list`

#### Response

- `200 OK`

```json
    {
    "users": [
 { "username1": ["spriteHeadshot URL", "isOnline (boolean)", "isOwner (boolean)"] },
        { "username2": ["spriteHeadshot URL", "isOnline (boolean)", "isOwner (boolean)"] },
        { "username3": ["spriteHeadshot URL", "isOnline (boolean)", "isOwner (boolean)"] },
    ]
    }
```

### GetAgentListIncubating

#### Description

This endpoint returns agent information for the "Incubating" list screen.

#### Request

- `GET /worlds/{worldId}/agents/list/incubating`

#### Response

- `200 OK`

```json
    {
    "incubating": [
 { "userId1": ["username1", "spriteHeadshot URL", "TotalIncubationTime", "IncubationTimeLeft"] },
        { "userId2": ["username2", "spriteHeadshot URL", "TotalIncubationTime", "IncubationTimeLeft"] },
        { "userId3": ["username3", "spriteHeadshot URL", "TotalIncubationTime", "IncubationTimeLeft"] }
    ]
    }
```

### GetAgentListHatched

#### Description

This endpoint returns agent information for the "Hatched" list screen.

#### Request

- `GET /worlds/{worldId}/agents/list/hatched`

#### Response

- `200 OK`

```json
    {
    "hatched": [
 { "userId1": "username1", "spriteHeadshot URL" },
        { "userId2": "username2", "spriteHeadshot URL" },
        { "userId3": "username3", "spriteHeadshot URL" }
    ]
    }
```

### KickUser

#### Description

This endpoint is used to kick a player from a world (important: only the owner of the world has the privilege to kick someone).

#### Request

- `DELETE /worlds/{worldId}/users/{userId}`

```json
    {
    "ownerId": "userToRemoveId"
    }
```

#### Response

- `200 OK`

### BroadcastUserLogin - IMPORTANT: LEKINA TO IMPLEMENT WITH SIGNAL R - THIS IS A GAME SERVICE TO FRONT END MESSAGE

#### Description

When the back-end receives notice that a new user has logged into the server, the back-end will send out a SingalR message to all other users about the login and the respective user's info.

#### Response

`SIGNAL R MESSAGE - TO DO: SET MESSAGE`

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
        "sprite_headshot_URL" : "string"
    }
```

### BroadcastUserLogOut - IMPORTANT: LEKINA TO IMPLEMENT WITH SIGNAL R - THIS IS A GAME SERVICE TO FRONT END MESSAGE

#### Description

When the back-end receives notice from the front-end that an online user has logged off the server, the back-end will send out a SingalR message to all other users about the log-out.

#### Response

`SIGNAL R MESSAGE - TO DO: SET MESSAGE`

```json
    {
        "userId": "00000-00000-00000-00000-00000"
    }
```

## User Endpoints

### GetUser

#### Description

This endpoint returns user information for the given `userId`.

#### Request

- `GET /users/{userId}`

#### Response

- `200 OK`

  ```json
  {
    "username": "string",
    "email": "string",
    "lastKnownX": 0,
    "lastKnownY": 0,
    "isOnline": true,
    "createdTime": "2023-11-04T22:54:19.911Z",
    "lastActiveTime": "2023-11-04T22:54:19.911Z"
  }
  ```

### GetUserWorlds

#### Description

Returns the list of worlds that a user belongs to - for display on home screen.

#### Request

- `GET /users/{userId}/worlds`

#### Response

- `200 OK`

```json
  {
    "worlds": [
 { "worldId1": ["worldName1", "description", [ "playerId1", "playerId2", "playerId3" ]] },
        { "worldId2": ["worldName2", "description", [ "playerId1", "playerId2", "playerId3"]] },
        { "worldId3": ["worldName3", "description", [ "playerId1 sprite", "playerId2 sprite", "playerId3 sprite"]] }
    ]
  }
  ```

- Note: When sending the playerId's to the front-end, the Game service will randomly pick up to 3 playerIds' sprites to send.

### RemoveUserWorld

#### Description

Removes a world from a user's list of worlds that they belong to.

#### Request

- `DELETE /users/{userId}/worlds/{worldId}`

#### Response

- `200 OK`

  ```json
  [
    "World with id {worldId} removed from user's list"
  ]
  ```

### UpdateUserSprite

#### Description

Updates a user's sprite - **TO DO: TALK TO EVAN TO FIGURE OUT IF WE ARE GOING TO PROVIDE A LINK OR A DESCRIPTION TO SEND TO THE AI TEAM GUYS. TALK TO FRONT-END/MOCK UP GUYS ABOUT THIS: MAYBE JUST HAVE SELECTABLE BUTTONS FOR APPEARNCE OR THEY CAN INPUT TEXT SUMMARY OF APPEARANCE.**

#### Request

- `POST /users/{userId}/sprite/`

```json
  [
    "userId": "summary of how they want avatar to look" OR "photo they upload"
  ]
```

#### Response

- `200 OK`

### UpdateLocation

#### Description

This method updates the user’s location in the map whenever a user changes location in the game.

#### Function prototype

```csharp
Task UpdateLocation(Location location);
public record Location(int X_coordinate, int Y_coordinate);
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

- This endpoint creates an AI Agent with the game server.

#### Request

- `POST /agents/create`

  ```json
  {
    "username": "string",
    "createdByUser": "user ID as string",
    "collaborationDurationInHours": 24,
    "description": "string",
  }
  ```

#### Response

- `200 OK`

  ```json
  {
    "id": "string",
    "ALAN TO REVIEW THIS": "string"
  }
  ```

### GetAgent

#### Description

This endpoint returns agent information for the given `agentId`.

#### Request

- `GET /agents/{agentId}`

#### Response

- `200 OK`

  ```json
  {
    "username": "string",
    "isHatched": "False",
    "createdByUser": "string",
    "createdTime": "2023-11-04T22:54:19.911Z",
    "summary": "string",
    "description": "string",
    "spriteHeadshot": "URL",
    "isHatched": "True/False",
    "IncubationTimeLeft": "5 hours",
    "TotalIncubationTime": "24 hours",
  }
  ```

### GetAgentSummary

#### Description

- This endpoint grabs the GPT summary generated for an AI agent.

#### Request

- `GET /agents/summary/{agentId}`

#### Response

- `200 OK`

  ```json
  {
    "agentId": "summary"
  }
  ```

## Chat Endpoints

### SendMessage

#### Description

This endpoint sends a message to a user/group through the server.

#### Function prototype

```csharp
Task SendMessage(Guid receiverId, string message);
```

#### Request

```csharp
Guid receiverId = Guid.NewGuid();
string message = "Example message"
HubConnection.SendAsync("SendMessage", receiverId, message)
```

#### Response

- The `SendMessage` callback on the server will forward the message to the `User` or `Group` matching the `receiverId`.

### DeleteChat

#### Description

This endpoint allows a user to delete a message sent to a user/group through the server.

#### Request

- `DELETE /chats/{chatId}`

#### Response

- `204 No Content`

### GetChatHistory

#### Description

This endpoint gets the chat history between the current client and another user/group with `receiverId`.

#### Request

- `GET /chats/history`

  ```json
  {
    "senderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "recipientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
  ```

#### Response

- `200 OK`

  ```json
  [
    {
      "Id": "5fw92f53-5717-4562-b3fc-2c963f66afa6",
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

Gets the IDs of all the chats sent by the user with ID `userId`.

#### Request

- `GET /chats?senderId={senderIdValue}`

#### Response

- `200 OK`

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

- This endpoint returns the questions a user responds to when initially creating their own character.

#### Request

- `GET /questions/users`

#### Response

- `200 OK`

  ```json
    {
    "questionId": "string",
    "questionId": "string",
    "questionId": "string"
    }
  ```

### GetAgentQuestions

#### Description

- This endpoint returns the questions for training an agent.

#### Request

- `GET /questions/agents`

#### Response

- `200 OK`

  ```json
  {
    "questionId": "string",
    "questionId": "string",
    "questionId": "string"
  }
  ```

### PostResponse

#### Description

This endpoint records the clients responses to either questions about themselves (user) or questions about an incubating agent.

#### Request

- `POST /questions/response/`

  ```json
  {
    "responderId": ["targetCharacterId", "questionId", "response"]
  }
  ```

- **ResponderID** is the ID of the user who is answering a question (either about themselves or an incubating agent).
- **TargetCharacterID** is the ID of the user or agent who is having questions answered about them. If a user is answering questions about themselves, the **ResponderID** and **TargetCharacterID** is the same.
- **Response** is the response to the question

#### Response

- `204 No Content`

### GetAllResponses

#### Description

- This endpoint returns all the questions and their corresponding responses about a specific user or agent.

#### Request

- `GET /questions/responses/{characterId}`

#### Response

- `200 OK`

```json
    {
    { "TargetCharacterId": "target_guid_here", "ResponderId": "responder_guid_here", "QuestionId": "question_guid_here", "Response": "Response 1" },
    { "TargetCharacterId": "target_guid_here", "ResponderId": "responder_guid_here", "QuestionId": "question_guid_here", "Response": "Response 2" },
    { "TargetCharacterId": "target_guid_here", "ResponderId": "responder_guid_here", "QuestionId": "question_guid_here", "Response": "Response 3" },
    { "TargetCharacterId": "target_guid_here", "ResponderId": "responder_guid_here", "QuestionId": "question_guid_here", "Response": "Response 4" },
    { "TargetCharacterId": "target_guid_here", "ResponderId": 5"responder_guid_here", "QuestionId": "question_guid_here", "Response": "Response 5" }
    }
```

- Note: There can be multiple responses for one question.

### GetResponse

#### Description

- This endpoint returns the responses for a specific question regarding a specific user or agent.

#### Request

- `GET /questions/responses/{targetCharacterId}/{questionId}`

#### Response

- `200 OK`

```json
    {
    { "TargetCharacterId": "same_target_guid_here", "ResponderId": "responder_guid_here", "QuestionId": "same_question_ID", "Response": "Response 1" },
    { "TargetCharacterId": "same_target_guid_here", "ResponderId": "diff_responder_guid_here", "QuestionId": "same_question_ID", "Response": "Response 2" },
    }
```

- Note: There can be multiple responses for one question.

### AddQuestion

#### Description

- This endpoint can be used to add a question into the database.

#### Request

- `POST /questions/`

```json
  {
    "questionText": "string",
    "questionType": "User/Agent/Both",
    "questionNum" // unclear if we need this field, but would be used to tell the front-end in which order to present each question on the screen.
  }
```

#### Response

- `200 OK`
