using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record CreateTicketIssuanceRequest(
    List<SeatPositionDto> SeatPositionList,
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);