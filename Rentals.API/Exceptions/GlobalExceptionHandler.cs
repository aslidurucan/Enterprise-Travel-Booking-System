using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Rentals.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .Select(x => new { Field = x.PropertyName, Message = x.ErrorMessage });

                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Doğrulama Hatası",
                    Detail = "Gönderdiğiniz veriler iş kurallarımıza uymuyor."
                };
                problem.Extensions.Add("errors", errors);
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }

            if (exception is UnauthorizedAccessException)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Yetkisiz Erişim",
                    Detail = exception.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }

            if (exception is KeyNotFoundException)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Kayıt Bulunamadı",
                    Detail = exception.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }

            if (exception is InvalidOperationException)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Çakışma",
                    Detail = exception.Message
                };
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
                return true;
            }

            _logger.LogError(exception, "Beklenmeyen bir hata oluştu.");

            var serverError = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Sunucu Hatası",
                Detail = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(serverError, cancellationToken);
            return true;
        }
    }
}
