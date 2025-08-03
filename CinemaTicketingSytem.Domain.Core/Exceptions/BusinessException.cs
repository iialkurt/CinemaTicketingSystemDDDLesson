using System.Net;

namespace CinemaTicketingSystem.SharedKernel.Exceptions;

public class BusinessException(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : Exception
{
    private readonly List<object> _placeHolderData = [];
    public string ErrorCode { get; private set; } = errorCode;
    public HttpStatusCode StatusCode { get; private set; } = statusCode;

    public IReadOnlyList<object> PlaceholderData => _placeHolderData;

    public static BusinessException Create(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new BusinessException(errorCode, statusCode);
    }

    public BusinessException AddData(object data)
    {
        _placeHolderData.Add(data);
        return this;
    }
}