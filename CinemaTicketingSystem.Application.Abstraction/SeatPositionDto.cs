using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Application.Abstraction;

public record SeatPositionDto(string Row, int Number, SeatType SeatType)
{
}
