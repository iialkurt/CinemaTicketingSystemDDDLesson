using System.Collections.ObjectModel;

namespace CinemaTicketingSystem.Domain;

public class AggregateRoot<T> : Entity<T>
{
    private readonly ICollection<DomainEvent> _domainEvents = new Collection<DomainEvent>();
    private readonly ICollection<DomainEvent> _integrationEvents = new Collection<DomainEvent>();


    public IEnumerable<DomainEvent> GetDomainEvents => _domainEvents;

    public IEnumerable<DomainEvent> GetIntegrationEvents()
    {
        return _integrationEvents;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }

    protected virtual void AddLocalEvent(object eventData)
    {
        _domainEvents.Add(new DomainEvent(eventData));
    }

    protected virtual void AddDistributedEvent(object eventData)
    {
        _integrationEvents.Add(new DomainEvent(eventData));
    }
}