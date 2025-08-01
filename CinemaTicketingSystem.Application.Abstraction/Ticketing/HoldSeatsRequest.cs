namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public record HoldSeatsRequest(List<SeatPositionDto> SeatPosition, Guid ScheduledMovieShowId);
