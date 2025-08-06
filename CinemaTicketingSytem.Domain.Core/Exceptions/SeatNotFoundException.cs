#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;

#endregion

namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class SeatNotFoundException : BusinessException
{
    public SeatNotFoundException(string row, int number)
        : base(ErrorCodes.SeatNotFound)
    {
        AddData(row);
        AddData(number.ToString());
    }
}