using EventList.WebApi.Common.Mappings;
using EventList.WebApi.Entities;
using EventList.WebApi.ValueObjects;

namespace EventList.WebApi.Features.Lectures.Dtos
{
    public sealed class UserLectureDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public IList<string?> LecturerNames { get; set; }

        public Location Location { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;

        public string? Name { get; set; }

        public string? Topic { get; set; }

        public string? Description { get; set; }
    }
}
