using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;

namespace CinemaTicketingSystem.Application.Ticketing;

public interface ISeatHoldAppService
{
    Task<AppResult> HoldSeats(HoldSeatsRequest request);
}