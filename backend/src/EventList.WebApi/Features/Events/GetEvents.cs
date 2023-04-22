using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventList.WebApi.Common;
using EventList.WebApi.Common.Mappings;
using EventList.WebApi.Entities;
using EventList.WebApi.Infrastructure.Persistence;
using EventList.WebApi.ValueObjects;
using log4net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Events;

public class GetEventsController : ApiControllerBase
{
    [HttpGet("/api/events")]
    [SwaggerOperation(Tags = new[] { "Events" })]
    public async Task<ActionResult<EventsVm>> Get()
    {
        return await Mediator.Send(new GetEventsQuery());
    }
}

public class GetEventsQuery : IRequest<EventsVm>
{
}

public class EventsVm
{
    public IList<EventDto> Events { get; set; } = new List<EventDto>();
}

public class EventDto : IMapFrom<Event>
{
    public EventDto()
    {
        Lectures = new List<LectureDto>();
    }

    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime StartDate { get; set; }

    public IList<LectureDto> Lectures { get; set; }
}

public class LectureDto : IMapFrom<Lecture>
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public IList<Lecturer> Lecturers { get; set; }

    public string? Location { get; set; }

    public DateTime StartTime { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Name { get; set; }

    public string? Topic { get; set; }

    public string? Description { get; set; }

    public bool Finished { get; set; }
}

public class LecturerDto : IMapFrom<Lecturer>
{
    public int Id { get; set; }

    public int LectureId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}


internal sealed class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, EventsVm>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEventsQueryHandler> _logger;

    public GetEventsQueryHandler(ApplicationDbContext context, IMapper mapper, ILogger<GetEventsQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EventsVm> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get Events.");
        return new EventsVm
        {
            Events = await _context.Events
                .AsNoTracking()
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken)
        };
    }
}
