using EventList.WebApi.Common;
using System.Drawing;

namespace EventList.WebApi.Entities
{
    public class Event : AuditableEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }

        public IList<Lecture> Lectures { get; private set; } = new List<Lecture>();
    }
}
