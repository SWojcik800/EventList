using EventList.WebApi.Common;
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
            Finished = false;
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

        private bool _finished;
        public bool Finished
        {
            get => _finished;
            set
            {
                if (value && _finished == false)
                {
                    DomainEvents.Add(new LectureFinishedEvent(this));
                }

                _finished = value;
            }
        }

        public Event Event { get; set; } = null!;

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    }

    public class LectureFinishedEvent : DomainEvent
    {
        public LectureFinishedEvent(Lecture lecture)
        {
            Lecture = lecture;
        }

        public Lecture Lecture { get; }

    }
}
