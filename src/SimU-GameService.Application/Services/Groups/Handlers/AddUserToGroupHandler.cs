using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Groups.Handlers;

public class AddUserToGroupHandler : IRequestHandler<AddUserToGroupCommand, Unit>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public AddUserToGroupHandler(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
    {
        // get group and user
        var group = await _groupRepository.GetGroup(request.GroupId) ?? throw new NotFoundException(nameof(Group), request.GroupId);
        var user = await _userRepository.GetUser(request.UserId) ?? throw new NotFoundException(nameof(User), request.UserId);

        // check if the client making the request is the owner of the group
        // for now, only the owner of the group can add users to the group
        if (group.OwnerId != request.RequesterId)
        {
            throw new BadRequestException($"User with ID {request.RequesterId} is not the owner of group with ID {request.GroupId}");
        }

        // check if user is already in group
        if (group.MemberIds.Contains(request.UserId))
        {
            throw new BadRequestException($"User with ID {request.UserId} is already in group with ID {request.GroupId}");
        }

        // add user to the group
        await _groupRepository.AddUser(group.GroupId, user.UserId);
        return Unit.Value;
    }
}