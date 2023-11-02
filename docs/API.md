# API

## Table of Contents

- [Notes](#notes)
- [Authentication Endpoints](#authentication-endpoints)
- [User Endpoints](#user-endpoints)
- [Group Endpoints](#group-endpoints)
- [Chat Endpoints](#chat-endpoints)

## Notes

- We will use a combination of REST API and SignalR requests to fulfill the API contract. SignalR is solely used for instances where we might need to send/broadcast information to other clients in the game.
- You might notice that the SignalR requests do not include the sender's ID. We will resolve the sender's identity using the connection ID to the server's SignalR hub.
- This document will probably evolve (gradually). In case of connection errors, please refer to this document as the initial source of truth.

## Authentication Endpoints

### RegisterUser

#### Description

This endpoint registers (and logs in) a user with the game server.

#### Function prototype

```csharp
Task<AuthenticationResponse> RegisterUserAsync(
  string firstName, 
  string lastName, 
  string email, 
  string password);
public record AuthenticationResponse(Guid Id, string AuthToken);
```

#### Request

- `POST {{host}}/authentication/register`

```json
{
  "firstName" : "John",
  "lastName" : "Doe",
  "email" : "john.doe@example.com",
  "password" : "MY_VERY_STRONG_PASSWORD"
}
```

#### Response

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "authToken": "eyJhbGciOiJIUzI1...k6yJV_adQssw5c"
}
```

### LoginUser

#### Description

This endpoint logs in a user with the game server.

#### Function prototype

```csharp
Task<AuthenticationResponse> LoginUserAsync(string email, string password);
public record AuthenticationResponse(Guid Id, string AuthToken);
```

#### Request

- `POST {{host}}/authentication/login`

```json
{
  "email" : "JohnDoe@example.com",
  "password" : "MY_VERY_STRONG_PASSWORD"
}
```

#### Response

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "authToken": "eyJhbGciOiJIUzI1...k6yJV_adQssw5c"
}
```

### LogoutUser

#### Description

This endpoint logs out the user from the game.

#### Function prototype

```csharp
Task LogoutUser(Guid userId);
```

#### Request

`PUT {{host}}/authentication/{userId}/logout`

#### Response

`204 No Content`

## User Endpoints

### GetQuestionnaire

#### Description

This endpoint returns the user entrance questionnaire to the client.

#### Function prototype

```csharp
Task<IEnumerable<string>> GetQuestionnaire();
```

#### Request

- `GET {{host}}/user/questionnnaire`

#### Response

```json
[
  "Example question",
  "Another example question"
]
```

### PostQuestionResponses

#### Description

This method records the clients responses from the entrance questionnaire.

#### Function prototype

```csharp
Task PostQuestionResponses(Guid userId, List<string>);
```

#### Request

- `POST {{host}}/user/{userId}/questionnnaire`

```json
[
  "Example response string",
  "Another example response string"
]
```

#### Response

- `204 No Content`

### UpdateLocation

#### Description

This method updates the user’s location in the map whenever a user changes location in the game.

#### Function prototype

```csharp
Task UpdateLocation(Location location);
public record Location(int X_coordinate, Y_coordinate);
```

#### Request

- SignalR code

```csharp
Location location = new Location(0, 0);
HubConnection.SendAsync("UpdateLocation", location.ToString())
```

#### Response

The `UpdateLocation` callback on the server will broadcast the user’s new location to all other clients using the client-side `UpdateLocation` callback as shown below:

```csharp
await Clients.All.SendAsync("UpdateLocation",
    userId.ToString(),
    location.ToString());
```

### GetUser

#### Description

This method returns user information for the given `userId`.

#### Function prototype

```csharp
Task<UserResponse> GetUser(Guid userId);
public record UserResponse(
 string FirstName,
 string LastName,
 string Email,
 bool IsLoggedIn,
 DateTime CreatedTime,
 DateTime LastActiveTime,
 Location LastKnownLocation);
```

#### Request

- `GET {{host}}/user/{userId}`

#### Response

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "isLoggedIn" : true,
  "createdTime": "2023-01-01T00:00:00",
  "lastActiveTime" : "2023-01-01T00:00:00",
  "lastKnownLocation": {
      "x_coordinate": 0,
      "y_coordinate": 0
    }
}
```

### SendFriendRequest

#### Description

This endpoint allows a user to send a friend request to another user with ID `userId` in the game.

#### Function prototype

```csharp
Task SendFriendRequest(Guid userId);
```

#### Request

- SignalR code

```csharp
Guid userId = Guid.Empty();
await HubConnection.SendAsync("SendFriendRequest", userId.ToString());
```

#### Response

The `SendFriendRequest` server callback will handle sending the friend request to the user with ID `userId`.

### RespondToFriendRequest

#### Description

This endpoint allows a user to accept or decline a friend request from another user with ID `userId` in the game.

#### Function prototype

```csharp
Task RespondToFriendRequest(Guid userId, bool accepted);
```

#### Request

- SignalR code

```csharp
Guid userId = Guid.Empty();
HubConnection.SendAsync("RespondToFriendRequest", userId.ToString(), true);
```

#### Response

The `RespondToFriendRequest` server callback will handle creating a `Friend` object containing the two user IDs if the friend request was accepted and notifying the sender of the friend request of the outcome.

### RemoveFriend

#### Description

This endpoint allows a user with ID `userId` to terminate a *friendship* with another user with ID `friendId` in the game.

#### Function prototype

```csharp
Task RemoveFriend(Guid friendId);
```

#### Request

- `DELETE {{host}}/user/{userId}/friends`

```json
{
  "friendId" : "00000000-0000-0000-0000-000000000000"
}
```

#### Response

- `204 No Content`

### GetFriends

#### Description

This endpoint returns the list of friends that a certain user with ID `userId` has befriended.

#### Function prototype

```csharp
Task<IEnumerable<Friend>> GetFriends(userId);
public record Friend(Guid friendId, DateTime createdTime);
```

#### Request

- `GET {{host}}/user/{userId}/friends`

#### Response

```json
[
  {
    "friendId" : "00000000-0000-0000-0000-000000000000",
    "createdTime": "2022-01-01T00:00:00"
  },
  {
    "friendId" : "00000000-0000-0000-0000-000000000000",
    "createdTime": "2022-01-01T00:00:00"
  }
]
```

## Group Endpoints

### CreateGroup

#### Description

Any client with ID `ownerId` uses this endpoint to create a new group with a given `name`.

#### Function prototype

```csharp
Task<Guid> CreateGroup(Guid ownerId, string name);
```

#### Request

- `POST {{host}}/group`

```json
{
  "ownerId" : "00000000-0000-0000-0000-000000000000",
  "name" : "Example group name"
}
```

#### Response

```json
{
  "groupId" : "00000000-0000-0000-0000-000000000000",
  "createdTime": "2022-01-01T00:00:00"
}
```

### JoinGroup

#### Description

A client with ID `userId` uses this endpoint to request to join a group with ID `groupId`.

#### Function prototype

```csharp
Task JoinGroup(Guid groupId, Guid userId);
```

#### Request

- SignalR code

```csharp
await HubConnection.SendAsync("JoinGroup", groupId.ToString());
```

#### Response

The `JoinGroup` server callback prompts the owner of the group with ID `groupId` to add the `userId` to the group.

### AddUser

#### Description

A client with ID `ownerId` uses this endpoint to add a new member with ID `userId` to a group with ID `groupId`.

#### Function prototype

```csharp
Task AddUser(Guid groupId, Guid ownerId, Guid userId);
```

#### Request

- SignalR code

```csharp
await HubConnection.SendAsync("AddUser", groupId.ToString(), ownerId.ToString(), userId.ToString());
```

#### Response

The `AddUser` server callback adds the `userId` to the group and notifies the user of the change.

### RemoveUser

#### Description

- Remove a user from a group.

#### Function prototype

```csharp
Task RemoveUser(Guid groupId, Guid ownerId, Guid userId);
```

#### Request

- SignalR code

```csharp
await HubConnection.SendAsync("RemoveUser", groupId.ToString(), ownerId.ToString(), userId.ToString());
```

#### Response

The `RemoveUser` server callback removes the `userId` from the group and (if applicable) notifies the user of the change.

### DeleteGroup

#### Description

This endpoint deletes the group with ID `groupId` from the game server.

#### Function prototype

```csharp
Task DeleteGroup(Guid groupId, Guid ownerId);
```

#### Request

- `DELETE {{host}}/group/{groupId}`

```json
{
  "ownerId" : "00000000-0000-0000-0000-000000000000"
}
```

#### Response

- `204 No Content`

## Chat Endpoints

### SendChat

#### Description

This endpoint sends a message to a user/group through the server.

#### Function prototype

```csharp
Task SendChat(Guid receiverId, string message);
```

#### Request

- SignalR code

```csharp
Guid receiverId = Guid.Empty();
string message = "Example message"
HubConnection.SendAsync("SendMessage", receiverId.ToString(), message)
```

#### Response

- The `SendMessage` callback on the server will forward the message to the `User` or `Group` matching the `receiverId`.

### DeleteChat

#### Description

This endpoint allows a user with ID `userId` to delete a message sent to a user/group through the server.

#### Function prototype

```csharp
Task DeleteChat(Guid chatId, Guid userId);
```

#### Request

- `DELETE {{host}}/chat/{chatId}`

```json
{
  "userId" : "00000000-0000-0000-0000-000000000000"
}
```

#### Response

- `204 No Content`

### GetChatHistory

#### Description

This endpoint gets the chat history between the current client and another user/group with `receiverId`.

#### Function prototype

```csharp
Task<IEnumerable<ChatResponse>> GetChatHistory(Guid senderId, Guid receiverId);
public record ChatResponse(
 Guid SenderId,
 Guid ReceiverId,
 string Content,
 bool IsGroupChat,
 DateTime CreatedTime);
```

#### Request

- `GET {{host}}/chats/history`

```json
{
  "senderId" : "00000000-0000-0000-0000-000000000000",
  "receiverId" : "00000000-0000-0000-0000-000000000000"
}
```

#### Response

```json
[
  {
    "SenderID": "00000000-0000-0000-0000-000000000000",
    "ReceiverID": "00000000-0000-0000-0000-000000000000",
    "Content": "Example chat message",
    "IsGroupChat": false,
    "CreatedTime": "2023-01-01T00:00:00"
  },
  {
    "SenderID": "00000000-0000-0000-0000-000000000000",
    "ReceiverID": "00000000-0000-0000-0000-000000000000",
    "Content": "Another example chat message",
    "IsGroupChat": false,
    "CreatedTime": "2023-01-01T00:00:00"
  }
]
```

### GetUserChats

#### Description

Gets the IDs of all the chats sent by the user with ID `userId`.

#### Function prototype

```csharp
Task<IEnumerable<ChatResponse>> GetUserChats(Guid userId);
```

#### Request

- `GET {{host}}/chats`

```json
{
  "userId" : "00000000-0000-0000-0000-000000000000"
}
```

#### Response

```json
[
  {
    "SenderID": "00000000-0000-0000-0000-000000000000",
    "ReceiverID": "00000000-0000-0000-0000-000000000000",
    "Content": "Example chat message",
    "IsGroupChat": true,
    "CreatedTime": "2023-01-01T00:00:00"
  },
  {
    "SenderID": "00000000-0000-0000-0000-000000000000",
    "ReceiverID": "00000000-0000-0000-0000-000000000000",
    "Content": "Another example chat message",
    "IsGroupChat": false,
    "CreatedTime": "2023-01-01T00:00:00"
  }
]
```
