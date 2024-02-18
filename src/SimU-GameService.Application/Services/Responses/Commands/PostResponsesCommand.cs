using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Responses.Commands;

public record PostResponsesCommand(Guid TargetCharacterId, IEnumerable<Response> Responses) : IRequest<string>;