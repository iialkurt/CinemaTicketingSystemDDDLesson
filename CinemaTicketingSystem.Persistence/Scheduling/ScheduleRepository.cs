#region

using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Scheduling;

internal class ScheduleRepository(AppDbContext context)
    : GenericRepository<Schedule>(context), IScheduleRepository
{
    public Task<List<Schedule>> GetMoviesByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default)
    {
        return context.Schedules
            .Where(x => x.HallId == hallId).OrderBy(x => x.ShowTime.StartTime)
            .ToListAsync(cancellationToken);
    }
}