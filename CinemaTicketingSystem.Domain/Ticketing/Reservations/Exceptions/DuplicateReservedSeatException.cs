using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class DuplicateReservedSeatException(SeatNumber seatNumber)
    : BusinessException($"Seat {seatNumber} cannot be reserved twice in the same reservation.",
        "Reservation.DuplicateSeat")
{
    public SeatNumber SeatNumber { get; } = seatNumber;
}