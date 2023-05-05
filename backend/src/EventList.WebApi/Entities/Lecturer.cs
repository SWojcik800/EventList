using EventList.WebApi.Common;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace EventList.WebApi.Entities
{
    public sealed class Lecturer : AuditableEntity
    {
        private Lecturer()
        {
            //For EF Core 
        }

        private static readonly Regex _whitespaceRegex = new Regex(@"\s+");
        private string _name;

        public Lecturer(string? name, string? description)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; set; }

        public IList<Lecture> Lectures { get; private set; }

        public string? Name { 
            get => _name; 
            set
            {
                _name = NormalizeName(value);
            } 
        }

        public string? Description { get; set; }

        public Event Event { get; set; } = null!;

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void UnassignLecturerFromAll()
        {
            DomainEvents.Add(new LecturerUnassignedFromAllEvent(Id, Lectures));
        }

        public void UpdateLectures(IList<Lecture> lectures)
        {
            Lectures = lectures;
        }

        private string NormalizeName(string inputName)
        {
            var normalizedName = _whitespaceRegex.Replace(inputName.Trim(), " ");
            return normalizedName;
        }
        public sealed class LecturerUnassignedFromAllEvent : DomainEvent
        {
            public int LecturerId { get; }
            public IList<Lecture> Lectures { get; }
            public LecturerUnassignedFromAllEvent(int lecturerId, IList<Lecture> lectures) : base()
            {
                LecturerId = lecturerId;
                Lectures = lectures;

            }
        }
    }
}
