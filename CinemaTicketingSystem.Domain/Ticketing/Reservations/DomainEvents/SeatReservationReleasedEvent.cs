namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public class SeatReservationReleasedEvent : IDomainEvent
{
    public SeatReservationReleasedEvent(Guid reservationId, SeatNumber seatNumber)
    {
        ReservationId = reservationId;
        SeatNumber = seatNumber;
    }

    public Guid ReservationId { get; }
    public SeatNumber SeatNumber { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}