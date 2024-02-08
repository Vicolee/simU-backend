# LLM Service API

## Notes

- Check the docs on the [AI Service repo](https://github.com/dartmouth-cs98-23f/SimYou_LLM_Service/blob/main/Docs/API.md) for updates.

## SendChat

### Description

The GameService sends the AI service a chat message from an online player whose intended recipient is an AI agent / offline player. The AI service retrieves relevant memories to construct a prompt that it sends to the OpenAI API. The AI serviece sends the GameService the response and the GameService adds the chat response to the chat table in the back-end database and sends along the response to the user.

### Request

`POST /api/agents/prompt`

```json
{
  "senderId": "00000000-0000-0000-0000-000000000000",
  "recipientId": "00000000-0000-0000-0000-000000000000",
  "conversationID": "00000000-0000-0000-0000-000000000000",
  "content": "Test message",
  "streamResponse": false,
  "respondWithQuestion": false
}
```

### Response

```json
{
  "content": "Test response"
}
```

## EndConversation

### Description

At the end of any conversation taking place in the game, notify the AI service. This will allow the AI service to generate summaries of the conversation which can be stored in the vector database collections for both of the actors involved in the conversation.

### Request

`POST /api/agents/endconversation`

```json
{
  "conversationID": "00000000-0000-0000-0000-000000000000",
  "participants": [
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000"
  ]
}
```

### Response

`No content`

## GenerateSprite (*Stretch Feature*)

### Description

The GameService receives either a picture of the real person's face or a description of what the user wants their sprite's appearance to look like. If it's a picture, it will be sent to the AI Service to be described by GPT into words. Then the description will be sent along to DALL-E to generate a full body sprite and a headashot of the sprite, which will then be sent back to the GameService, saved by it, and sent along to the Front-End.

### Request

`POST ${generate_sprite_url}`

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "description": "Test description",
  "photo_URL": "https://www.test.com/test_photo.png"
}
```

### Response

```json
{
  "sprite_URL": "https://www.test.com/test_sprite.png",
  "spriteHeadshot_URL": "https://www.test.com/test_sprite_headshot.png"
}
```

## GenerateCharacterSummary

### Description

The GameService receives the question responses about a new user or a new AI Agent. It then sends those responses along to the AI Service which passes it to GPT to generate a summary of the user / agent. The AI service then returns this summary to the Game Service.

### Request

`POST /api/agents/generatepersona`

```json
{
  "characterId": "00000000-0000-0000-0000-000000000000",
  "questions": [
    "question 1",
    "question 2",
    "question 3"
  ],
  "responses": [
    ["response to q1", "response to q1" ],
    ["response to q2", "response to q2" ],
    ["response to q3", "response to q3" ]
  ]
}
```

### Response

```json
{
  "summary": "Test summary"
}
```

## GenerateWorldThumbnail

### Description

AI service uses generative AI to create a thumbnail for a world to be displayed on the front end.

### Request

`POST /api/thumbnails`

```json
{
  "worldId": "00000000-0000-0000-0000-000000000000",
  "creatorId": "00000000-0000-0000-0000-000000000000",
  "description": "Test description"
}
```

### Response

```json
{
  "thumbnailURL": "https://www.test.com/test_thumbnail.png"
}
```
