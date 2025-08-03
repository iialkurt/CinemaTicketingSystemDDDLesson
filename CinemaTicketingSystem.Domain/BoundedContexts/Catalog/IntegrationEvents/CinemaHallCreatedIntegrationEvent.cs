using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;

public record CinemaHallCreatedIntegrationEvent(Guid HallId, ScreeningTechnology hallTechnology, short SeatCount) : IIntegrationEvent
{
}