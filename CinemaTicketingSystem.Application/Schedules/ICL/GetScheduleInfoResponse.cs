#region

using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public record GetScheduleInfoResponse(Guid CinemaHallId, Guid MovieId, ShowTime showTime, Price TicketPrice);