using MediatR;

using Microsoft.Extensions.Logging;

using EventList.WebApi.Common.Models;
using EventList.WebApi.Entities;

namespace EventList.WebApi.Features.Lectures.EventHandlers;

public class LectureFinishedEventHandler : INotificationHandler<DomainEventNotification<LectureFinishedEvent>>
{
    private readonly ILogger<LectureFinishedEventHandler> _logger;

    public LectureFinishedEventHandler(ILogger<LectureFinishedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<LectureFinishedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("EventList Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}