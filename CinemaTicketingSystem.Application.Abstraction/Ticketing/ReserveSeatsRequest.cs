namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record ReserveSeatsRequest(List<SeatPositionDto> SeatPositionList, Guid ScheduledMovieShowId);