#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.DomainEvents;

public record TicketUsedEvent(Guid TicketId, Guid CustomerId, DateTime UsedAt) : IDomainEvent;