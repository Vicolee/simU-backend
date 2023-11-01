# API

*Note*: For real-time requests and responses, we get the `senderId` from the SignalR hub connection and not the client request.

## RegisterUser

### Description

This endpoint registers a user account with the game server.

### Function prototype

```csharp
Task<AuthenticationResponse> RegisterUserAsync(string firstName, string lastName, string email, string password);
public record AuthenticationResponse(Guid Id, string AuthToken);
```

### Request

- `POST {{host}}/authentication/register`

```json
{
  "firstName" : "John",
  "lastName" : "Doe",
  "email" : "john.doe@example.com",
  "password" : "MY_VERY_STRONG_PASSWORD"
}
```

### Response

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "authToken": "eyJhbGciOiJIUzI1...k6yJV_adQssw5c"
}
```

## LoginUser

### Description

This endpoint logs in a user with the game server.

### Function prototype

```csharp
Task<AuthenticationResponse> LoginUserAsync(string email, string password);
public record AuthenticationResponse(Guid Id, string AuthToken);
```

### Request

- `POST {{host}}/authentication/login`

```json
{
  "email" : "JohnDoe@example.com",
  "password" : "MY_VERY_STRONG_PASSWORD"
}
```

### Response

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "authToken": "eyJhbGciOiJIUzI1...k6yJV_adQssw5c"
}
```

## GetQuestionnaire

### Description

This endpoint returns the user entrance questionnaire to the client.

### Function prototype

```csharp
Task<IEnumerable<string>> GetQuestionnaire()
```

### Request

- `GET {{host}}/user/questionnnaire`

### Response

```json
[
  {
    "index" : "0",
    "question": "Example question"
  },
  {
    "index" : "1",
    "question": "Another example question"
  }
]
```

## PostQuestionResponses

### Description

This method records the clients responses from the entrance questionnaire.

### Function prototype

```csharp
Task AddQuestionResponses(Guid userId, List<QuestionnaireResponse>);
public record QuestionnaireResponse(int questionIndex, string responseString);
```

### Request

- `POST {{host}}/user/{userId}/questionnnaire`

```json
[
  {
    "index" : "0",
    "responseString": "Example response string"
  },
  {
    "index" : "1",
    "responseString": "Another example response string"
  }
]
```

### Response

- `200 OK` (No content)

## SendChatMessage

### Description

This method sends a message to a user/group through the server.

### Function prototype

```csharp
Task SendMessage(Guid receiverId, string message);
```

### Request

- SignalR code

```csharp
Guid receiverId = Guid.Empty();
string message = "Example message"
HubConnection.SendAsync("SendMessage", receiverId.ToString(), message)
```

```json
{
  "receiverId" : "00000000-0000-0000-0000-000000000000",
  "message" : "Example message"
}
```

### Response

- *Action*: The `SendMessage` callback on the server will forward the message to the `User` or `Group` matching the `receiverId`.

## GetChatHistory

### Description

This endpoint gets the chat history between the current client and another user/group with `correspondentId`.

### Function prototype

```csharp
Task<IEnumerable<ChatResponse>> GetChatHistory(Guid userId, Guid correspondentId);
public record ChatResponse(
 Guid SenderId,
 Guid ReceiverId,
 string Content,
 bool IsGroupChat,
 DateTime CreatedTime);
```

### Request

- `GET {{host}}/chats/{userId}/history`

```json
{
  "correspondentId" : "00000000-0000-0000-0000-000000000000"
}
```

### Response

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

## UpdateUserLocation

### Description

This method updates the user’s location in the map whenever a user changes location in the game.

### Function prototype

```csharp
Task UpdateLocation(Location location);
public record Location(int X_coordinate, Y_coordinate);
```

### Request

- SignalR code

```csharp
Location location = new Location(0, 0);
HubConnection.SendAsync("UpdateLocation", location.ToString())
```

```json
{
    "x_coordinate" : "0",
  "y_coordinate" : "0"
}
```

### Response

The `UpdateLocation` callback on the server will broadcast the user’s new location to all other clients using the client-side `UpdateUserLocation` callback as shown in the example below:

```csharp
public Task UpdateLocation(string location){
 var userId = MapConnectionIdToUserID(context.connectionId);
 await Clients.All.SendAsync("UpdateUserLocation",
    userId.ToString(),
    location.ToString());
}
```

## GetUser

### Description

This method returns user information for the given `userId`.

### Function prototype

```csharp
Task<UserResponse> GetUser(Guid userId);
public record UserResponse(
 string FirstName,
 string LastName,
 string Email,
 DateTime CreatedTime,
 Location LastKnownLocation);
```

### Request

- `GET {{host}}/user/{userId}`

### Response

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "createdTime": "2023-01-01T00:00:00",
  "lastKnownLocation": {
      "x_coordinate": 0,
      "y_coordinate": 0
    }
}
```
