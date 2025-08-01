namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record ReservationCreatedEvent(
    Guid ReservationId,
    Guid CustomerId,
    Guid ScheduledMovieShowId,
    DateTime ReservationTime)
    : IDomainEvent;