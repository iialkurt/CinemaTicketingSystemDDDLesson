using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record SeatReservationReleasedEvent(Guid ReservationId, SeatPosition SeatPosition) : IDomainEvent;