namespace CinemaTicketingSystem.Domain.Catalog.DomainEvents;

public record MovieShowingEndedEvent(Guid MovieId, string MovieTitle, DateTime EndDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}