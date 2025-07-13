namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public class ReservationCanceledEvent : IDomainEvent
{
    public ReservationCanceledEvent(Guid reservationId, Guid customerId, Guid movieSessionId)
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