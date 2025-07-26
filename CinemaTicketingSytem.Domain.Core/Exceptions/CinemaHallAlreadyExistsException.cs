namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class CinemaHallAlreadyExistsException : BusinessException
{
    public CinemaHallAlreadyExistsException(string cinemaHallName) : base(ErrorCodes.CinemaHallAlreadyExists)
    {
        AddData(cinemaHallName);
    }
}