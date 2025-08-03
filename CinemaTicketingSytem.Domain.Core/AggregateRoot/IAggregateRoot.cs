namespace CinemaTicketingSystem.SharedKernel.AggregateRoot;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
    void AddDomainEvent(IDomainEvent eventData);



    IReadOnlyCollection<IIntegrationEvent> IntegrationEvents { get; }

    void ClearIntegrationEvents();
    void AddIntegrationEvent(IIntegrationEvent eventData);
}