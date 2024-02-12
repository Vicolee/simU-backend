using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetLocationQuery(Guid UserId) : IRequest<Location?>;