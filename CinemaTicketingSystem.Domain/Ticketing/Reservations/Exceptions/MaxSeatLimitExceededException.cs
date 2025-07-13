using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Reservations.Exceptions;

public class MaxSeatLimitExceededException(int maxAllowed)
    : BusinessException($"Cannot reserve more than {maxAllowed} seats at once.", "Reservation.MaxSeatLimitExceeded")
{
    public int MaxAllowed { get; } = maxAllowed;
}