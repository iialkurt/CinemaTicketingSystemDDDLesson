namespace CinemaTicketingSystem.SharedKernel
{
    public interface IDomainEventBus
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
    }
}
