using MediatR;
using SimU_GameService.Application.Common.Abstractions;
using SimU_GameService.Application.Services.Groups.Commands;

namespace SimU_GameService.Application.Services.Groups.Handlers;
public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Unit>
{
    private readonly IGroupRepository _groupRepository;

    public DeleteGroupHandler(IGroupRepository groupRepository) => _groupRepository = groupRepository;

    public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        await _groupRepository.DeleteGroup(request.GroupId);
        return Unit.Value;
    }
}