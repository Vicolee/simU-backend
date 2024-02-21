using MediatR;

namespace SimU_GameService.Application.Services.Users.Commands;

public record LogoutUserCommand(Guid UserId) : IRequest<Unit>;