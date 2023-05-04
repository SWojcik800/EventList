using EventList.WebApi.Entities;

namespace EventList.WebApi.Features.Lecturers.Dtos
{
    public class LecturerDto
    {
        public int Id { get; set; }

        public IList<string>? LectureNames { get; private set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
