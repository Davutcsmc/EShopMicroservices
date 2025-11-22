using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler
{     
    public class CustomExceptionHandler 
        (ILogger<CustomExceptionHandler> logger): IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(
                "Error Message: {Message}, Time of occurence {time}, StackTrace: {StackTrace}", 
                exception.Message, DateTime.UtcNow, exception.StackTrace);

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InvalidServerException ex =>
                (
                    ex.Details ?? ex.Message,
                    ex.GetType().Name,
                    StatusCodes.Status500InternalServerError
                ),
                FluentValidation.ValidationException =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   StatusCodes.Status400BadRequest
                ),
                System.ComponentModel.DataAnnotations.ValidationException =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   StatusCodes.Status400BadRequest
                ),
                BadRequestException ex =>
                (
                   ex.Details ?? ex.Message,
                   ex.GetType().Name,
                   StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                   exception.Message,
                   exception.GetType().Name,
                   StatusCodes.Status404NotFound
                ),
                _ =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status500InternalServerError
                )
            };

            httpContext.Response.StatusCode = details.StatusCode;

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Status = details.StatusCode,
                Detail = details.Detail,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is FluentValidation.ValidationException validationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
