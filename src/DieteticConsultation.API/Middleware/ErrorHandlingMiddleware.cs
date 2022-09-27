using System.Net;
using DieteticConsultationAPI.Exceptions;

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
            catch (NotFoundException notFoundException)
            {
                await BuildResponse(context, notFoundException, HttpStatusCode.NotFound);
            }
            catch (CommonException ex)
            {
                // there is no need to log that something bad happened, here is place tha you should inform user about problem
                await BuildResponse(context, ex, HttpStatusCode.InternalServerError);
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