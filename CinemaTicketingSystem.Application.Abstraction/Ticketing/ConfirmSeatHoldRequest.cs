namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record ConfirmSeatHoldRequest(Guid ScheduledMovieShowId, DateOnly ScreeningDate);