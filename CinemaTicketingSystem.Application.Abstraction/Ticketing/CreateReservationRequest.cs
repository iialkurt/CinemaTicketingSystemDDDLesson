namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record CreateReservationRequest(
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);