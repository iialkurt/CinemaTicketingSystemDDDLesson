namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record CreateTicketIssuanceRequest(Guid ScheduledMovieShowId, DateOnly ScreeningDate);

public record CreateTicketIssuanceResponse(Guid CreatedTicketIssuanceId);