using DieteticConsultationAPI.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Net;

namespace DieteticConsultationAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware

    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ForbiddenResourceException forbiddenException)
            {
                await BuildResponse(context, forbiddenException, HttpStatusCode.BadRequest);
            }
            catch (IncorrectLogginException incorrectLogginException)
            {
                await BuildResponse(context, incorrectLogginException, HttpStatusCode.NotFound);
            }
            catch (CannotCreateResourceException cannotCreateResourceException)
            {
                await BuildResponse(context, cannotCreateResourceException, HttpStatusCode.BadRequest);
            }
            catch (CannotFindResourceException cannotFindResourceException)
            {
                await BuildResponse(context, cannotFindResourceException, HttpStatusCode.NotFound);
            }
            catch (CannotUploadResourceException cannotUploadResourceException)
            {
                await BuildResponse(context, cannotUploadResourceException, HttpStatusCode.NotFound);
            }
            catch (CommonHttpException ex)
            {
                await BuildResponse(context, ex, ex.Code);
            }
            catch (Exception)
            {
                await BuildResponse(context, "Something bad happened to our server", HttpStatusCode.InternalServerError);
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
