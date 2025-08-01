namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record ReservationConfirmedEvent(Guid ReservationId, Guid CustomerId, Guid ScheduledMovieShowId) : IDomainEvent;