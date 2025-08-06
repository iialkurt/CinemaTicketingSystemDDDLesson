#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;

public record ReservationConfirmedEvent(Guid ReservationId, Guid CustomerId, Guid ScheduledMovieShowId) : IDomainEvent;