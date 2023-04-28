using EventList.WebApi.Common;

namespace EventList.WebApi.Entities
{
    public class Lecturer : AuditableEntity
    {
        public int Id { get; set; }

        public IList<Lecture> Lectures { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        

    }
}
