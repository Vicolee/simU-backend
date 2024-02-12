using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Responses.Commands;

public record PostResponsesCommand(IEnumerable<Response> Responses) : IRequest<Unit>;