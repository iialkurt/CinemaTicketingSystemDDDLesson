namespace CinemaTicketingSystem.Domain.Reservations.DomainEvents;

public class SeatReservedEvent : IDomainEvent
{
    public SeatReservedEvent(Guid reservationId, SeatNumber seatNumber, Guid customerId)
    {
        ReservationId = reservationId;
        SeatNumber = seatNumber;
        CustomerId = customerId;
    }

    public Guid ReservationId { get; }
    public SeatNumber SeatNumber { get; }
    public Guid CustomerId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}