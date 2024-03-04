using MediatR;

namespace SimU_GameService.Api.DomainEvents.Events;

public record UserLoggedInEvent(Guid UserId) : INotification;