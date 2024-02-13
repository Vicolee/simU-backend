using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetSummaryQuery(Guid UserId) : IRequest<object?>;