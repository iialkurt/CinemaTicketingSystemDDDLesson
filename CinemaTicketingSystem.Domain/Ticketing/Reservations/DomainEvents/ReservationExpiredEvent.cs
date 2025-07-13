namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public class ReservationExpiredEvent : IDomainEvent
{
    public ReservationExpiredEvent(Guid reservationId, Guid customerId, Guid movieSessionId)
    {
        ReservationId = reservationId;
        CustomerId = customerId;
        MovieSessionId = movieSessionId;
    }

    public Guid ReservationId { get; }
    public Guid CustomerId { get; }
    public Guid MovieSessionId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}