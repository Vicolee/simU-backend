using MediatR;

namespace SimU_GameService.Api.DomainEvents.Events;

public record UserRemovedFromWorldEvent(Guid UserId) : INotification;