using MediatR;

namespace SimU_GameService.Api.DomainEvents.Events;

public record UserAddedToWorldEvent(Guid UserId) : INotification;