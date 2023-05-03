using EventList.WebApi.Common;

namespace EventList.WebApi.Entities
{
    public sealed class Lecturer : AuditableEntity
    {
        private Lecturer()
        {
            //For EF Core 
        }

        public Lecturer(string? name, string? description)
        {
            Name = name;
            Description = description;
        }

        public int Id { get; set; }

        public IList<Lecture> Lectures { get; private set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
