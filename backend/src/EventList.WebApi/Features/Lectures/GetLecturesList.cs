using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventList.WebApi.Common;
using EventList.WebApi.Common.Interfaces;
using EventList.WebApi.Features.Lectures.Dtos;
using EventList.WebApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Lectures
{
    #region Api Endpoints
    public sealed class GetLecturesListController : ApiControllerBase
    {
        [HttpGet("/api/lectures")]
        [SwaggerOperation(Tags = new[] { "Lectures" })]
        public async Task<ActionResult<GetLecturesListViewModel>> GetAll()
        {
            var dtos = await Mediator.Send(new GetLecturesListQuery());
            return Ok(new GetLecturesListViewModel()
            {
                Items = dtos.ToList()
            });
        }
    }

    public sealed class GetLecturesListViewModel
    {
        public IList<UserLectureDto> Items { get; set; }
    }
    #endregion
    #region Query definition
    public sealed class GetLecturesListQuery : IRequest<IEnumerable<UserLectureDto>>
    {

    }
    #endregion

    #region Query handler
    public sealed class GetActiveLecturesListRequestHandler : IRequestHandler<GetLecturesListQuery, IEnumerable<UserLectureDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTime _dateTimeProvider;
        private readonly IMapper _mapper;

        public GetActiveLecturesListRequestHandler(
            ApplicationDbContext context,
            IDateTime dateTimeProvider,
            IMapper mapper)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserLectureDto>> Handle(GetLecturesListQuery request, CancellationToken cancellationToken)
        {
            var lectureDtos = await _context.Lectures.AsNoTracking()
                .Include(l => l.Lecturers)
                .Where(l => l.EndTime >= _dateTimeProvider.Now)
                .ProjectTo<UserLectureDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return lectureDtos;


        }
    }

    #endregion
}
