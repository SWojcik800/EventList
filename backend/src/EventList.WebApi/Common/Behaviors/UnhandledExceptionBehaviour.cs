using EventList.WebApi.Common.Exceptions;
using MediatR;

namespace EventList.WebApi.Common.Behaviors
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (ValidationException ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex,
                    "EventList Request: Validation Exception for Request {Name} {@Request} \n Validation Messages: {Messages}",
                    requestName, request, ex.FormattedValidationErrors());

                throw;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "EventList Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
