using EventList.WebApi.Common.Interfaces;
using MediatR.Pipeline;

namespace EventList.WebApi.Common.Behaviors;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? string.Empty;

        return Task.Run(() => _logger.LogInformation("EventList Request: {Name} {@UserId} {@Request}",
                requestName, userId, request), cancellationToken);
    }
}