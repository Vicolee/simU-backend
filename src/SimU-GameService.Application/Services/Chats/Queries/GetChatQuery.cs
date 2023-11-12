using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Chats.Queries;

public record GetChatQuery(Guid ChatId) : IRequest<Chat?>;