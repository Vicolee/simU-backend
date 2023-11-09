using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Groups.Handlers;

public class CreateGroupHandler : IRequestHandler<CreateGroupCommand, Guid>
{
    private readonly IGroupRepository _groupRepository;

    public CreateGroupHandler(IGroupRepository groupRepository) => _groupRepository = groupRepository;

    public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var group = new Group(request.Name, request.OwnerId);
        await _groupRepository.AddGroup(group);
        return group.Id;
    }
}