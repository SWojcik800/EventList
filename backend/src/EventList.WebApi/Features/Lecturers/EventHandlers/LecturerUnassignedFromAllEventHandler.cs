using Microsoft.Extensions.Logging;
using EventList.WebApi.Common.Models;
using MediatR;
using static EventList.WebApi.Entities.Lecturer;
using EventList.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventList.WebApi.Features.Lecturers.EventHandlers
{
    public class LecturerUnassignedFromAllEventHandler : INotificationHandler<DomainEventNotification<LecturerUnassignedFromAllEvent>>
    {
        private readonly ILogger<LecturerUnassignedFromAllEventHandler> _logger;
        private readonly ApplicationDbContext _context;

        public LecturerUnassignedFromAllEventHandler(ILogger<LecturerUnassignedFromAllEventHandler> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Handle(DomainEventNotification<LecturerUnassignedFromAllEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Event List Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            foreach (var lecture in notification.DomainEvent.Lectures)
            {
                // TODO: remove lecturer from lectures
                // fix migrations
            }
            

            return Task.CompletedTask;
        }
    }
}