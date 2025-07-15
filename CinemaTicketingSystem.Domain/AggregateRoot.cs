namespace CinemaTicketingSystem.Domain;

public class AggregateRoot<T> : Entity<T>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


    protected virtual void AddDomainEvent(IDomainEvent eventData)
    {
        _domainEvents.Add(eventData);
    }


}