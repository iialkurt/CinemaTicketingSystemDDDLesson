using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class InvalidReservationStateException(ReservationStatus currentStatus, string attemptedAction)
    : BusinessException($"Cannot {attemptedAction} when reservation is in '{currentStatus}' state.",
        "Reservation.InvalidState")
{
    public ReservationStatus CurrentStatus { get; } = currentStatus;
    public string AttemptedAction { get; } = attemptedAction;
}