using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public record GetScheduleInfoResponse(Guid CinemaHallId, Guid MovieId, ShowTime showTime);