using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Domain.Models;
using SimU_GameService.Application.Services.Users.Queries;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class GetLocationHandler : IRequestHandler<GetLocationQuery, Location?>
{
    private readonly ILocationRepository _locationRepository;

    public GetLocationHandler(ILocationRepository locationRepository) => _locationRepository = locationRepository;

    public async Task<Location?> Handle(
        GetLocationQuery request,
        CancellationToken cancellationToken) => await _locationRepository.GetLocation(request.UserId);
}