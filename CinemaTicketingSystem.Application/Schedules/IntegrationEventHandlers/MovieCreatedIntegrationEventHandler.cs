using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Application.Schedules.IntegrationEventHandlers;

public class MovieCreatedIntegrationEventHandler(
    IGenericRepository<Guid, MovieSnapshot> movieScheduleRepository,
    IUnitOfWork unitOfWork) : IIntegrationEventHandler<MovieCreatedIntegrationEvent>
{
    public async Task HandleAsync(MovieCreatedIntegrationEvent message, CancellationToken cancellationToken = default)
    {
        var movieSnapshot = new MovieSnapshot(message.MovieId, message.Duration, message.Technology);
        await movieScheduleRepository.AddAsync(movieSnapshot, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}