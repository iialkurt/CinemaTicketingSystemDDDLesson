#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases.DomainEvents;

public record TicketPurchasedEvent(
    Guid TicketId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition,
    Price Price) : IDomainEvent;