using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;

public record MovieCreatedIntegrationEvent(Guid MovieId, Duration Duration, ScreeningTechnology Technology) : IIntegrationEvent
{
}