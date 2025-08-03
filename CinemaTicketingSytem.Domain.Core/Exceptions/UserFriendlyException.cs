using System.Net;

namespace CinemaTicketingSystem.SharedKernel.Exceptions;

public class UserFriendlyException(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : Exception
{
    private readonly List<string> PlaceHolderData = [];
    public string ErrorCode { get; private set; } = errorCode;

    public IReadOnlyList<string> PlaceholderData => PlaceHolderData;

    public HttpStatusCode StatusCode { get; set; } = statusCode;

    public static UserFriendlyException Create(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new UserFriendlyException(errorCode)
        {
            StatusCode = statusCode
        };
    }

    public UserFriendlyException AddData(string data)
    {
        PlaceHolderData.Add(data);
        return this;
    }
}