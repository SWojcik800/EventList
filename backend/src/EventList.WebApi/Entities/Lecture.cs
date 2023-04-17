using EventList.WebApi.Common;
using EventList.WebApi.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventList.WebApi.Entities
{
    public class Lecture : AuditableEntity, IHasDomainEvent
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public IList<Lecturer> Lecturers { get; private set; } = new List<Lecturer>();

        public Location Location { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

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

        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.Ignore(e => e.DomainEvents);
        }
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
