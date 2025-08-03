using CinemaTicketingSystem.SharedKernel;
using MassTransit;

namespace CinemaTicketingSystem.ServiceBus;

public class IntegrationEventBus(IPublishEndpoint publishEndpoint) : IIntegrationEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent
    {
        return publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}