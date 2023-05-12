using AutoMapper;
using EventList.WebApi.Entities;
using EventList.WebApi.Features.Lecturers.Dtos;

namespace EventList.WebApi.Features.Lecturers.Mapping
{
    public class LecturerMapping : Profile
    {
        public LecturerMapping()
        {
            CreateMap<Lecturer, LecturerDto>()
                .ForMember(dest => dest.LectureNames, opt => opt.MapFrom(x => x.Lectures.Select(x => x.Name).ToList()));
        }
    }
}
