using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimU_GameService.Application.Common.Exceptions;

namespace SimU_GameService.Api.Middleware
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = HandleException(context.Exception);
            context.ExceptionHandled = true;
        }

        private static ObjectResult HandleException(Exception? exception)
        {
            var statusCode = exception switch
            {
                NotImplementedException => HttpStatusCode.NotImplemented,
                NotFoundException => HttpStatusCode.NotFound,
                BadRequestException => HttpStatusCode.BadRequest,
                ServiceErrorException => HttpStatusCode.ServiceUnavailable,
                _ => HttpStatusCode.InternalServerError
            };

            var errorObj = new { Error = exception?.Message ?? "An error occurred while processing your request." };
            return new ObjectResult(errorObj)
            {
                StatusCode = (int?)statusCode
            };
        }
    }
}