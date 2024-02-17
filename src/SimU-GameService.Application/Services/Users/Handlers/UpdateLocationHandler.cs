using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Services.Users.Commands;

namespace SimU_GameService.Application.Services.Users.Handlers;

public class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand, Unit>
{
    private readonly ILocationRepository _locationRepository;

    public UpdateLocationHandler(ILocationRepository locationRepository) => _locationRepository = locationRepository;

    public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        await _locationRepository.UpdateLocation(request.UserId, request.XCoord, request.YCoord);
        return Unit.Value;
    }
}