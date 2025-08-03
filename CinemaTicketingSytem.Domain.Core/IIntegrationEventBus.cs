namespace CinemaTicketingSystem.SharedKernel
{
    public interface IIntegrationEventBus
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IIntegrationEvent;
    }
}
