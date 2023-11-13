using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Users.Queries;

public record GetUserLocationQuery(Guid LocationId) : IRequest<Location?>;