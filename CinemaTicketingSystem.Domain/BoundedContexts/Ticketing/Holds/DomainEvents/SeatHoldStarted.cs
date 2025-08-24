#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.DomainEvents;

internal record SeatHoldStarted(
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    DateOnly ScreeningDate,
    SeatPosition seatPosition) : IDomainEvent;