namespace CinemaTicketingSystem.SharedKernel.Exceptions;

public class SeatAlreadyExistsException : BusinessException
{
    public SeatAlreadyExistsException(string row, int number)
        : base(ErrorCodes.SeatAlreadyExists)
    {
        AddData(row);

        AddData(number.ToString());
    }
}