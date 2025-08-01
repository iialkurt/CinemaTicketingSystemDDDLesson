using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Repositories;

namespace CinemaTicketingSystem.Persistence.Ticketing;

internal class PurchaseRepository(AppDbContext context)
    : GenericRepository<Guid, Purchase>(context), IPurchaseRepository
{
    public List<Purchase> GetTicketsPurchaseByScheduleId(Guid scheduleId)
    {
        return _context.TicketPurchases.Where(x => x.ScheduledMovieShowId == scheduleId).ToList();
    }
}