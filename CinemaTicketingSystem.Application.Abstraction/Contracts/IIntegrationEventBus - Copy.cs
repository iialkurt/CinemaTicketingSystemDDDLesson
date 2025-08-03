using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Abstraction.Contracts
{
    public interface IDomainEventBus
    {
        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
    }
}
