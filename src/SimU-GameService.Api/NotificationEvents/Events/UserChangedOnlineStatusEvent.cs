using MediatR;

namespace SimU_GameService.Api.DomainEvents.Events;

public record UserChangedOnlineStatusEvent(Guid UserId, bool IsOnline) : INotification;