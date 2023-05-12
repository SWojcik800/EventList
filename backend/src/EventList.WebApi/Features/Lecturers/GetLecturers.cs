using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventList.WebApi.Common;
using EventList.WebApi.Common.Interfaces;
using EventList.WebApi.Features.Lecturers.Dtos;
using EventList.WebApi.Features.Lectures;
using EventList.WebApi.Features.Lectures.Dtos;
using EventList.WebApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Lecturers
{
    #region Api Endpoints
    public sealed class GetLecturersController : ApiControllerBase
    {
        [HttpGet("/api/lecturers")]
        [SwaggerOperation(Tags = new[] { "Lecturers" })]
        public async Task<ActionResult<GetLecturersViewModel>> GetAll()
        {
            var dtos = await Mediator.Send(new GetLecturersQuery());
            return Ok(new GetLecturersViewModel()
            {
                Items = dtos.ToList()
            });
        }

        public sealed class GetLecturersViewModel
        {
            public IList<LecturerDto> Items { get; set; }
        }
        #endregion

        #region Query definition
        public sealed class GetLecturersQuery : IRequest<IEnumerable<LecturerDto>>
        {

        }
        #endregion

        #region Query handler
        public sealed class GetLecturersRequestHandler : IRequestHandler<GetLecturersQuery, IEnumerable<LecturerDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IDateTime _dateTimeProvider;
            private readonly IMapper _mapper;

            public GetLecturersRequestHandler(
                ApplicationDbContext context,
                IDateTime dateTimeProvider,
                IMapper mapper)
            {
                _context = context;
                _dateTimeProvider = dateTimeProvider;
                _mapper = mapper;
            }
            public async Task<IEnumerable<LecturerDto>> Handle(GetLecturersQuery request, CancellationToken cancellationToken)
            {
                var lecturerDtos = await _context.Lecturers.AsNoTracking()
                    .Include(l => l.Lectures)
                    .ProjectTo<LecturerDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return lecturerDtos;


            }
        }

        #endregion

    }
}
