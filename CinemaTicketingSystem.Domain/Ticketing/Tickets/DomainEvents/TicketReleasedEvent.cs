namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.DomainEvents;

public record TicketReleasedEvent(Guid TicketId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}