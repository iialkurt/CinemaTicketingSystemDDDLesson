namespace CinemaTicketingSystem.SharedKernel.Exceptions;

public class CinemaHallNotFoundException : BusinessException
{
    public CinemaHallNotFoundException(Guid hallId)
        : base(ErrorCodes.CinemaHallNotFound)
    {
        AddData(hallId.ToString());
    }
}