using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Abstraction.Contracts;

public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}