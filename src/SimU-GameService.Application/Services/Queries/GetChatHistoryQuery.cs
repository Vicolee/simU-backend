using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Queries;

public record GetChatHistoryQuery(Guid UserA_Id, Guid UserB_Id) : IRequest<IEnumerable<Chat>>;