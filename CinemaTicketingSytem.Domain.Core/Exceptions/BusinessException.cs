namespace CinemaTicketingSystem.Domain.Core.Exceptions;

public class BusinessException(string errorCode) : Exception
{
    private readonly List<object> _placeHolderData = [];
    public string ErrorCode { get; private set; } = errorCode;

    public IReadOnlyList<object> PlaceholderData => _placeHolderData;


    public static BusinessException Create(string errorCode)
    {
        return new BusinessException(errorCode);
    }

    public BusinessException AddData(object data)
    {
        _placeHolderData.Add(data);
        return this;
    }
}