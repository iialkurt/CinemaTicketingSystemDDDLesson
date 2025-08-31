using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents
{
    public record PurchaseCreatedIntegrationEvent(UserId userId, Guid TicketIssuanceId) : IIntegrationEvent;
}