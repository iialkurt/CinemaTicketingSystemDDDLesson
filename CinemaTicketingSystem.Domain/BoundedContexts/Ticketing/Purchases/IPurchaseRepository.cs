using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;

public interface IPurchaseRepository : IGenericRepository<Purchase>
{
    List<Purchase> GetTicketsPurchaseByScheduleId(Guid scheduleId);
}