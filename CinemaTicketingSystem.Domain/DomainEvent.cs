namespace CinemaTicketingSystem.Domain;

public class DomainEvent(object eventData)
{
    public object EventData { get; } = eventData;
}