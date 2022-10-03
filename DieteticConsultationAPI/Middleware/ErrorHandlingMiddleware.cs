using DieteticConsultationAPI.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Net;

namespace DieteticConsultationAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware

    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ForbidException)
            {
                context.Response.StatusCode = 403;
            }
            catch (BadReguestException badRequestException)
            {
                await BuildResponse(context, badRequestException, HttpStatusCode.BadRequest);
            }
            catch (NotFoundHttpException notFoundException)
            {
                await BuildResponse(context, notFoundException, HttpStatusCode.NotFound);
            }
            catch (CommonHttpException ex)
            {
                await BuildResponse(context, ex, ex.Code);
            }
            catch (Exception)
            {
                await BuildResponse(context, "Something bas happened to our server", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task BuildResponse(HttpContext context, Exception ex, HttpStatusCode code)
        {
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(ex.Message);
        }

        private static async Task BuildResponse(HttpContext context, string message, HttpStatusCode code)
        {
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(message);
        }
    }
}