using AutoMapper;
using EventList.WebApi.Entities;
using EventList.WebApi.Features.Lectures.Dtos;

namespace EventList.WebApi.Features.Lectures.Mapping
{
    public sealed class LectureMapping : Profile
    {

        public LectureMapping()
        {
            CreateMap<Lecture, UserLectureDto>()
                .ForMember(dest => dest.LecturerNames, opt => opt.MapFrom(x => x.Lecturers.Select(x => x.Name).ToList()))
                .ForMember(dest => dest.Duration, opt => opt.Ignore());
        }
    }
}
