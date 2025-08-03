using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.API.Extensions;

public class BusinessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BusinessException domainException) return false;
        var problemDetails = new ProblemDetails();

        var errorCode = domainException.ErrorCode;
        var placeHolderList = domainException.PlaceholderData;

        var localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();

        var title = placeHolderList.Any() ? localizer.L(errorCode, placeHolderList.ToArray()) : localizer.L(errorCode);

        problemDetails.Title = title;
        problemDetails.Status = (int)domainException.StatusCode;
        httpContext.Response.StatusCode = (int)domainException.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        return true;
    }
}