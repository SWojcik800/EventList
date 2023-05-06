using Microsoft.Extensions.Logging;
using EventList.WebApi.Common.Models;
using MediatR;
using static EventList.WebApi.Entities.Lecturer;
using EventList.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EventList.WebApi.Entities;
using EventList.WebApi.Common.Exceptions;
using Azure.Core;

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
            var lecturer = _context.Lecturers.Include(l => l.Lectures).Where(x => x.Id == domainEvent.LecturerId).FirstOrDefault();

            if (lecturer is null)
                throw new NotFoundException("Leturer", domainEvent.LecturerId);

            lecturer.Lectures.Clear();

            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}