namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public interface IReservationAppService
{
    Task<AppResult> ReserveSeats(ReserveSeatsRequest request);
}