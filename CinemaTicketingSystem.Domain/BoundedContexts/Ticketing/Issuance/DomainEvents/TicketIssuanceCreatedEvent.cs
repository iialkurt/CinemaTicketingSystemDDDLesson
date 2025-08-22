#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.DomainEvents;

public record TicketIssuanceCreatedEvent(
    Guid TicketId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition,
    Price Price) : IDomainEvent;