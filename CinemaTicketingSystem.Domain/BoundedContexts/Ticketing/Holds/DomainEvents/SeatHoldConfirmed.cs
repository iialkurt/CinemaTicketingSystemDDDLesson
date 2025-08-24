#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.DomainEvents;

public record SeatHoldConfirmed(
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    DateOnly ScreeningDate,
    SeatPosition seatPosition) : IDomainEvent;