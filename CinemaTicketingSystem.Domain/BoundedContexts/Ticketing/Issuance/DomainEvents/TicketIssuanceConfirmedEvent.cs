using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.DomainEvents;

public record TicketIssuanceConfirmedEvent(Guid TicketId) : IDomainEvent;