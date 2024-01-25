# AI Service to GameService API

## Notes

## GameService to AI Service Communication

### RelayChatToAgent

#### Description

- The GameService sends the AI service a chat message from an online player whose intended recipient is an AI agent / offline player. The AI service then relays the message to the GPT service and sends the GameService the response. The GameService then adds the chat response to the chat table in the back-end database and sends along the response to the user.
- TO DO: UPDATE THE SEND CHAT HANDLER WITH PROPER FUNCTION NAMES AND WITH AGENT REPOSITORY ADDED
- TO DO: MAKE SURE BACK-END CODE CAN SUPPORT STREAMING OF RESPONSE

#### Request

-`RelayChatToAgent(chat.Id, chat.Content, chat.SenderId, chat.RecipientId);`

  ```json
  {
    "chatId": "GUID GameService generates",
    "content": "message",
    "senderId": "GUID of sender",
    "RecipientId": "GUID of recipient"
  }
  ```

#### Response

- `200 OK`

  ```json
  {
    "AgentResponderId": "ID of recipient who sent the message that triggered the response",
    "userRecipientId": "ID of user who originally sent the message that triggered this incoming",
    "content": "message"
  }
  ```

### RelayEndOfConversation

#### Description - UPDATE THIS: CONVERSATION ID - each chat message is assigned one. DO THIS FOR USER TO USER CONVERSATIONS AND USER TO AGENT CONVERSATIONS. THE MESSAGE WE SEND AI SERVICE WILL NOT INCLUDE THE CONTENT OF CONVERSATION - JUST INCLUDE A CONVERSATION ID FOR THEM TO QUERY OUR DATABASE.

- The GameService sends an alert to the AI service that a user sent another user a chat. This does not require a response from the GPT service, but the AI Service needs to be notified so that they can track the message in their vector database. Talk to the AI Service about if GameService should pass the chat directly to them or just send them the chat ID of the new message that was saved in the back-end database.

#### Request

-`UserToUserChatNotify(chat.Id, chat.SenderId, chat.RecipientId);`

  ```json
  {
    "chatId": "GUID GameService generates",
    "senderId": "GUID of sender",
    "RecipientId": "GUID of recipient"
  }
  ```

#### Response

- `200 OK`

### GenerateSprite

#### Description

- The GameService receives either a picture of the real person's face or a description of what the user wants their sprite's appearance to look like. If it's a picture, it will be sent to the AI Service to be described by GPT into words. Then the description will be sent along to DALL-E to generate a full body sprite and a headashot of the sprite, which will then be sent back to the GameService, saved by it, and sent along to the Front-End.

#### Request -- might not even need the userId

-`GenerateSprite(userId, description, photo);`

  ```json
  {
    "userId": "GUID of user",

    "description": "Description of appearance",
    or:
    "photo": "URL to photo that GameService saves in back-end"
  }
  ```

#### Response

- `200 OK`
  ```json
  {
    "userId": "GUID of user",
    "sprite": "sprite file",
    "spriteHeadshot": "sprite file",
  }
  ```

### GenerateSummary

#### Description

- The GameService receives the question responses about a new user or a new AI Agent. It then sends those responses along to the AI Service which passes it to GPT to generate a summary of the user / agent. The AI service then returns this summary to the Game Service.

#### Request

-`GenerateSummary(characterId, responses);`

  ```json
  {
    "characterId": "GUID of user/agent",

    "responses": [ {"question 1 content": ["response1 to q1", "response2 to q1"] }, {"question 2 content": ["response1 to q2", "response2 to q2" ] } ]
  }
  ```

#### Response

- `200 OK`
  ```json
  {
    "characterId": "GUID of user/agent",
    "generatedSummary": "summary"
  }
  ```

### GenerateWorldThumbnail

#### Description

- FILL IN - can just send the worldID, description of the world, and the creators ID

#### Request

-`GenerateWorldThumbnail(worldId, description, creatorId);`

  ```json
  {
    "worldId": "GUID of World",
    "description": "string",
    "creatorId": "GUID of user"
  }
  ```

#### Response

- `200 OK`
  ```json
  {
    "characterId": "GUID of user/agent",
    "generatedSummary": "summary"
  }
  ```


### GetConversationOpener - 