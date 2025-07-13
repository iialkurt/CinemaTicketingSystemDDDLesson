namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.DomainEvents;

public record TicketUsedEvent(Guid TicketId, Guid CustomerId, DateTime UsedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}