using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class CinemaHallNotFoundException : BusinessException
{
    public CinemaHallNotFoundException(Guid hallId)
        : base(ErrorCodes.CinemaHallNotFound)
    {
        AddData(hallId.ToString());
    }
}