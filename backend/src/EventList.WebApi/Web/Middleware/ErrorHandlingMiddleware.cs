using EventList.WebApi.Common.Exceptions;
using Newtonsoft.Json;

namespace EventList.WebApi.Web.Middleware
{
    /// <summary>
    /// Error handling middleware for web api
    /// </summary>
    public sealed class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

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
            catch (ValidationException e)
            {
                await WriteErrorListToBody(context, e.Errors, 400);
            }
            catch (ForbiddenAccessException e)
            {
                await WriteErrorToBody(context, e.Message, 403);
            }
            catch (ApplicationErrorException e)
            {
                await WriteErrorToBody(context, e.Message, 500);
            }
            catch (Exception e)
            {
                await WriteErrorToBody(context, e.Message, 500);
            }

        }

        private async Task WriteErrorToBody(HttpContext context, string errorMessage, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            var errorDict = new Dictionary<string, string[]>();
            errorDict.Add("Value", new string[] { errorMessage });
            await context.Response.WriteAsJsonAsync(new
            {
                errors = errorDict
            });
        }

        private async Task WriteErrorListToBody(HttpContext context, IDictionary<string, string[]> errorsList, int statusCode)
        {
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                errors = errorsList
            });
        }
    }
}
