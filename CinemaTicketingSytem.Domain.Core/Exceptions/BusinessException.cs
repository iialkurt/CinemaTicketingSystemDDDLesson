namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class BusinessException(string errorCode) : Exception
{
    private readonly List<string> PlaceHolderData = [];
    public string ErrorCode { get; private set; } = errorCode;

    public IReadOnlyList<string> PlaceholderData => PlaceHolderData;


    public static BusinessException Create(string errorCode)
    {
        return new BusinessException(errorCode);
    }

    public BusinessException AddData(string data)
    {
        PlaceHolderData.Add(data);
        return this;
    }
}