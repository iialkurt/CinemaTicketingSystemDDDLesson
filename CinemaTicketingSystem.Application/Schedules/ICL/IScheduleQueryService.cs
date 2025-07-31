using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public interface IScheduleQueryService
{
    Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId);
}