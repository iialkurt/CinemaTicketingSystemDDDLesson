#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;

public record SeatReservedEvent(
    Guid ReservationId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition) : IDomainEvent;