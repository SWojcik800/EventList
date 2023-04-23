using EventList.WebApi.Common.Exceptions;
using Newtonsoft.Json;

namespace EventList.WebApi.Web.Middleware
{
    /// <summary>
    /// Error handling middleware for web api
    /// </summary>
    public sealed class ErrorHandlingMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException e)
            {
                await WriteErrorToBody(context, e.Message, 404);
            }
            catch (ForbiddenAccessException e)
            {
                await WriteErrorToBody(context, e.Message, 403);
            }
            catch (ApplicationErrorException e)
            {
                await WriteErrorToBody(context, e.Message, 500);
            }

        }

        private async Task WriteErrorToBody(HttpContext context, string errorMessage, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            var apiError = new ErrorApiResponse(errorMessage);

            await context.Response.WriteAsJsonAsync(apiError);
        }
    }
}
