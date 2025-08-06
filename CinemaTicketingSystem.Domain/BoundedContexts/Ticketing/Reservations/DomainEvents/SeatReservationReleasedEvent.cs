#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record SeatReservationReleasedEvent(Guid ReservationId, SeatPosition SeatPosition) : IDomainEvent;