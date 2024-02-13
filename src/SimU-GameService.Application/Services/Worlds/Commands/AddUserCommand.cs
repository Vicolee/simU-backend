using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Worlds.Commands;

public record AddUserCommand(Guid WorldId, Guid UserId) : IRequest<World>;
