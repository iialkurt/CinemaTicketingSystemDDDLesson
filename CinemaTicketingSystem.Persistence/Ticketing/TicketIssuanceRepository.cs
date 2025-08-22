#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

internal class TicketIssuanceRepository(AppDbContext context)
    : GenericRepository<TicketIssuance>(context), ITicketIssuanceRepository
{
    public List<TicketIssuance> GetTicketsIssuanceByScheduleIdAndScreeningDate(Guid scheduleId, DateOnly ScreeningDate)
    {
        throw new NotImplementedException();
    }

    public List<TicketIssuance> GetTicketsPurchaseByScheduleIdAndScreeningDate(Guid scheduleId, DateOnly ScreeningDate)
    {
        return _context.TicketIssuance
            .Where(x => x.ScheduledMovieShowId == scheduleId && x.ScreeningDate == ScreeningDate)
            .ToList();
    }
}