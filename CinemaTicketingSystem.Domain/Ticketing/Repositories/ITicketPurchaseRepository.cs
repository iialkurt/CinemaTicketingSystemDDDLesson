using CinemaTicketingSystem.Domain.Ticketing.Tickets;

namespace CinemaTicketingSystem.Domain.Ticketing.Repositories;

public interface ITicketPurchaseRepository
{
    public List<TicketPurchase> GetTicketsPurchaseByScheduleId(Guid scheduleId);
}
