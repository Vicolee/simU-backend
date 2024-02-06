using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Common.Exceptions;
using SimU_GameService.Application.Services.Groups.Commands;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Groups.Handlers;

public class CreateGroupHandler : IRequestHandler<CreateGroupCommand, Guid>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public CreateGroupHandler(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var owner = await _userRepository.GetUser(request.OwnerId)
            ?? throw new NotFoundException(nameof(User), request.OwnerId);

        var group = new Group(request.Name, request.OwnerId);
        await _groupRepository.AddGroup(group);
        return group.Id;
    }
}