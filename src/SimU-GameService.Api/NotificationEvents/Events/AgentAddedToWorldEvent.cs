using MediatR;

namespace SimU_GameService.Api.DomainEvents.Events;

public record AgentAddedToWorldEvent(Guid AgentId, Guid CreatorId) : INotification;