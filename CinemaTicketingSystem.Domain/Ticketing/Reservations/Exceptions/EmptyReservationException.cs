using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class EmptyReservationException : BusinessException
{
    public EmptyReservationException() : base(TicketingErrorCodes.EmptyReservation)
    {
    }
}