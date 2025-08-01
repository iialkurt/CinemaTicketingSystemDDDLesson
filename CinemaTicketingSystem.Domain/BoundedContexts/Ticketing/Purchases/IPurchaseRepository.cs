using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Repositories;

public interface IPurchaseRepository : IGenericRepository<Guid, Purchase>
{
    List<Purchase> GetTicketsPurchaseByScheduleId(Guid scheduleId);
}
