using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.Exceptions;

public class MaxTicketLimitExceededException(int maxAllowed)
    : BusinessException($"Cannot purchase more than {maxAllowed} tickets at once.",
        TicketingErrorCodes.MaxTicketLimitExceeded)
{
    public int MaxAllowed { get; } = maxAllowed;
}