ValidationException diye bir exceptio tanımla ,bu exception middleware yakala geriye hata mesjlarını döndürsün
UserFriendlyException is a special type of business exception where you directly return a message to the user.


he constructor of BusinessException takes a few parameters, and all are optional. These are listed here:

code: A string value that is used as a custom error code for the exception. Client applications can check it while handling the exception and track the error type easily. You typically use different error codes for different exceptions. The error code can also be used to localize the exception, as we will see in the Localizing a business exception section.
message: A string exception message, if needed.
details: A detailed explanation message string, if needed.
innerException: An inner exception, if available. You can pass here if you have cached an exception and throw a business exception based on that exception.
logLevel: The logging level for this exception. It is an enum of the LogLevel type, and the default value is LogLevel.Warning.


throw new BusinessException(
    EventHubErrorCodes.OrganizationNameAlreadyExists
).WithData("Name", name);


As mentioned at the beginning of the Exception handling section, ABP automatically logs all exceptions. Business exceptions, authorization, and validation exceptions are logged with the Warning level, while other errors are logged with the Error level by default.

Returns 401 (unauthorized) if the user has not logged in, for AbpAuthorizationException
Returns 403 (forbidden) if the user has logged in, for AbpAuthorizationException
Returns 400 (bad request) for AbpValidationException
Returns 404 (not found) for EntityNotFoundException
Returns 403 (forbidden) for business and user-friendly exceptions
Returns 501 (not implemented) for NotImplementedException
Returns 500 (internal server error) for other exceptions (those are assumed to be infrastructure errors)


The soft-delete data filter
If you use the soft-delete pattern for an entity, you never delete the entity in the database physically. Instead, you mark it as deleted.

ABP defines the ISoftDelete interface to standardize the property to mark an entity as soft-delete. You can implement that interface for an entity, as shown in the following code block:

public class Order : AggregateRoot<Guid>, ISoftDelete
{
    public bool IsDeleted { get; set; }
    //...other properties
}