using CinemaTicketingSystem.Application.Abstraction.Contracts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaTicketingSystem.API.Extensions;

public class ArgumentExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ArgumentException argumentException) return false;

        var problemDetails = new ProblemDetails();
        var localizer = httpContext.RequestServices.GetRequiredService<ILocalizer>();


        var title = localizer.L("Common:ValidationError", "Validation failed");
        var detail = argumentException.Message;

        problemDetails.Title = title;
        problemDetails.Detail = detail;
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        // Parameter name varsa ekle
        if (!string.IsNullOrEmpty(argumentException.ParamName))
        {
            problemDetails.Extensions.Add("parameterName", argumentException.ParamName);
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
