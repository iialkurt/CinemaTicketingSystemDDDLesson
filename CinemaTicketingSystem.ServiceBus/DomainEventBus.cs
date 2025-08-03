using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.SharedKernel;
using MassTransit;

namespace CinemaTicketingSystem.ServiceBus;

public class DomainEventBus(IPublishEndpoint publishEndpoint) : IDomainEventBus
{
    public Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        return publishEndpoint.Publish(domainEvent, cancellationToken);
    }
}