namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public class ReservationCreatedEvent : IDomainEvent
{
    public ReservationCreatedEvent(Guid reservationId, Guid customerId, Guid movieSessionId, DateTime occurredOn)
    {
        ReservationId = reservationId;
        CustomerId = customerId;
        MovieSessionId = movieSessionId;
        OccurredOn = occurredOn;
    }

    public Guid ReservationId { get; }
    public Guid CustomerId { get; }
    public Guid MovieSessionId { get; }
    public DateTime OccurredOn { get; }
}