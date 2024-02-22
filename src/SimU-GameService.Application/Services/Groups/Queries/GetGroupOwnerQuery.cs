using MediatR;

namespace SimU_GameService.Application.Services.Groups.Queries;

public record GetGroupOwnerQuery(Guid GroupId) : IRequest<Guid>;