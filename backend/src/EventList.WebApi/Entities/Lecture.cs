using EventList.WebApi.Common;
using EventList.WebApi.Common.Interfaces;
using EventList.WebApi.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventList.WebApi.Entities
{
    public sealed class Lecture : AuditableEntity, IHasDomainEvent
    {
        private Lecture()
        {
            //For EF Core 
        }

        public Lecture(
            int eventId,
            IList<Lecturer> lecturers,
            Location location,
            DateTime startTime,
            DateTime endTime,
            string? name,
            string? topic,
            string? description,
            Event @event)
        {
            EventId = eventId;
            Lecturers = lecturers;
            Location = location;
            StartTime = startTime;
            EndTime = endTime;
            Name = name;
            Topic = topic;
            Description = description;
            Event = @event;
        }

        public int Id { get; set; }

        public int EventId { get; set; }

        public IList<Lecturer> Lecturers { get; private set; } = new List<Lecturer>();

        public Location Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Name { get; set; }

        public string? Topic { get; set; }

        public string? Description { get; set; }

        public bool IsFinished(IDateTime dateTimeProvider)
            => EndTime < dateTimeProvider.Now;

        public Event Event { get; set; } = null!;

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void ChangeLecturers(IList<Lecturer> lecturers)
        {
            Lecturers = lecturers;
            var changedIds = lecturers.Select(l => l.Id)
                .ToList();

            DomainEvents.Add(new LecturersChangedEvent(
                changedIds
            ));
        }

    }

    public sealed class LecturersChangedEvent : DomainEvent
    {
        public IReadOnlyCollection<int> LecturersIds { get;}
        public LecturersChangedEvent(IList<int> lecturersIds) : base()
        {
            LecturersIds = new List<int>(lecturersIds) { };
        }
    }
}
