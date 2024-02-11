using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Commands;

public record AddWorldCommand(Guid UserId, string JoinCode, bool IsOwner) : IRequest<World>;