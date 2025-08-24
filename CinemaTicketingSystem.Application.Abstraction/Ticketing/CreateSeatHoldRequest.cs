#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public record CreateSeatHoldRequest(
    List<SeatPositionDto> SeatPositions,
    Guid ScheduledMovieShowId,
    DateOnly ScreeningDate);