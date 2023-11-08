using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Queries;

public record GetUserChatsQuery(Guid UserId) : IRequest<IEnumerable<Chat>>;