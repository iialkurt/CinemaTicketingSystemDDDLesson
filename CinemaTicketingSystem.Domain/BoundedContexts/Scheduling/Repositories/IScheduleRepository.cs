#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    Task<List<Schedule>> GetMoviesByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default);
}