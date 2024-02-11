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

- This endpoint registers (and logs in) a user with the game server.

#### Request

- `POST /authentication/register`

  ```json
  {
    "firstName": "string",
    "lastName": "string",
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

### LogoutUser

#### Description

- This endpoint logs out the user from the game.

#### Request

- `PUT /authentication/{userId}/logout`

#### Response

- `204 No Content`

## User Endpoints

### GetQuestions

#### Description

- This endpoint returns the user entrance questionnaire to the client.

#### Request

- `GET /users/questions`

#### Response

- `200 OK`

  ```json
  [
    "string"
  ]
  ```

### PostResponses

#### Description

This endpoint records the clients responses from the entrance questionnaire.

#### Request

- `POST /users/{userId}/responses`

  ```json
  [
    "string"
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

### GetUser

#### Description

This endpoint returns user information for the given `userId`.

#### Request

- `GET /users/{userId}`

#### Response

- `200 OK`

  ```json
  {
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "lastKnownX": 0,
    "lastKnownY": 0,
    "isLoggedIn": true,
    "createdTime": "2023-11-04T22:54:19.911Z",
    "lastActiveTime": "2023-11-04T22:54:19.911Z"
  }
  ```

### InitalizeUserGameObjects()

#### Description

This endpoint returns a list of all users that are currently connected to the server. Typically, this function will be called when a user has just joined the server.

#### Request

- `GET /users`

#### Response

- `200 OK`

```json
{
  "userId1": { "locationX" = x, "locationY" = y, "characterSprite: null" },
  "userId2": { "locationX" = x, "locationY" = y, "characterSprite: null" }
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

```csharp
Guid userId = Guid.NewGuid();
await HubConnection.SendAsync("SendFriendRequest", userId);
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

```csharp
Guid userId = Guid.NewGuid();
HubConnection.SendAsync("RespondToFriendRequest", userId, true);
```

#### Response

The `RespondToFriendRequest` server callback will handle creating a `Friend` object containing the two user IDs if the friend request was accepted and notifying the sender of the friend request of the outcome.

### RemoveFriend

#### Description

This endpoint allows a user with ID `userId` to terminate a *friendship* with another user with ID `friendId` in the game.

#### Request

- `DELETE /users/{userId}/friends?friendId={friendIdValue}`

#### Response

- `204 No Content`

### GetFriends

#### Description

This endpoint returns the list of friends that a certain user with ID `userId` has befriended.

#### Request

- `GET /users/{userId}/friends`

#### Response

- `200 OK`

  ```json
  [
    {
      "friendId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "createdTime": "2023-11-04T23:00:27.828Z"
    }
  ]
  ```

## Group Endpoints

### CreateGroup

#### Description

Any client with ID `ownerId` uses this endpoint to create a new group with a given `name`.

#### Request

- `POST /group`

  ```json
  {
    "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string"
  }
  ```

#### Response

- `200 OK`

  ```json
  {
    "groupId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "createdTime": "2023-11-04T23:01:17.842Z"
  }
  ```

### JoinGroup

#### Description

A client uses this endpoint to request to join a group with ID `groupId`.

#### Function prototype

```csharp
Task JoinGroup(Guid groupId);
```

#### Request

```csharp
await HubConnection.SendAsync("JoinGroup", groupId);
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

```csharp
Guid groupId = Guid.NewGuid();
Guid userId = Guid.NewGuid();
Guid ownerId = Guid.NewGuid();

await HubConnection.SendAsync("AddUser", groupId, ownerId, userId);
```

#### Response

The `AddUser` server callback adds the `userId` to the group and notifies the user of the change.

### RemoveUser

#### Description

Remove a user from a group.

#### Function prototype

```csharp
Task RemoveUser(Guid groupId, Guid ownerId, Guid userId);
```

#### Request

```csharp
await HubConnection.SendAsync("RemoveUser", groupId, ownerId, userId);
```

#### Response

The `RemoveUser` server callback removes the `userId` from the group and (if applicable) notifies the user of the change.

### DeleteGroup

#### Description

This endpoint deletes the group with ID `groupId` from the game server.

#### Request

- `DELETE /group/{groupId}?ownerId={ownerIdValue}`

#### Response

- `204 No Content`

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
