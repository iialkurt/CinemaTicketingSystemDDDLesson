#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public interface IScheduleQueryService
{
    Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId);
}