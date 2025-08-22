using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record CreateSeatHoldRequest(
    List<SeatPositionDto> SeatPosition,
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);