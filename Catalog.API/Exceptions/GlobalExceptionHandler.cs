using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
           
            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .Select(x => new { Alan = x.PropertyName, Mesaj = x.ErrorMessage });

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Doğrulama Hatası (Validation Error)",
                    Detail = "Gönderdiğiniz veriler iş kurallarımıza uymuyor."
                };

                problemDetails.Extensions.Add("Errors", errors);

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }

            return false;
        }
    }
}
