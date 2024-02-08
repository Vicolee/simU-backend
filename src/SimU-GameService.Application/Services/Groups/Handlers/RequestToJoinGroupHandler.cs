using MediatR;
using SimU_GameService.Application.Abstractions.Repositories;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Groups.Handlers;

public class RequestToJoinGroupHandler : IRequestHandler<RequestToJoinGroupCommand, Guid>
{
    private readonly IGroupRepository _groupRepository;

    public RequestToJoinGroupHandler(IGroupRepository groupRepository) => _groupRepository = groupRepository;

    public async Task<Guid> Handle(RequestToJoinGroupCommand request, CancellationToken cancellationToken)
    {
        // get group from repository
        var group = await _groupRepository.GetGroup(request.GroupId)
            ?? throw new NotFoundException(nameof(Group), request.GroupId);

        // get owner ID from group
        return group.OwnerId;
    }
}