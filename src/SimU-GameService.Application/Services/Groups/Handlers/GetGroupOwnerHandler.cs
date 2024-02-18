using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Groups.Queries;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Groups.Handlers;

public class GetGroupOwnerHandler : IRequestHandler<GetGroupOwnerQuery, Guid>
{
    private readonly IGroupRepository _groupRepository;

    public GetGroupOwnerHandler(IGroupRepository groupRepository) => _groupRepository = groupRepository;

    public async Task<Guid> Handle(GetGroupOwnerQuery request, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroup(request.GroupId)
            ?? throw new NotFoundException(nameof(Group), request.GroupId);
        return group.OwnerId;
    }
}