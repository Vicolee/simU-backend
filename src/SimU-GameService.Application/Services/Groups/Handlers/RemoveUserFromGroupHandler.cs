using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Groups.Handlers;

public class RemoveUserFromGroupHandler : IRequestHandler<RemoveUserFromGroupCommand, Unit>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public RemoveUserFromGroupHandler(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(RemoveUserFromGroupCommand request, CancellationToken cancellationToken)
    {
        // get group and user
        var group = await _groupRepository.GetGroup(request.GroupId) ?? throw new NotFoundException(nameof(Group), request.GroupId);
        var user = await _userRepository.GetUser(request.UserId) ?? throw new NotFoundException(nameof(User), request.UserId);

        // check if the client making the request is the owner of the group
        // for now, only the owner of the group can remove users from the group
        if (group.OwnerId != request.RequesterId)
        { 
            throw new BadRequestException($"User with ID {request.RequesterId} is not the owner of group with ID {request.GroupId}");
        }

        await _groupRepository.RemoveUser(group.GroupId, user.UserId);
        return Unit.Value;
    }
}