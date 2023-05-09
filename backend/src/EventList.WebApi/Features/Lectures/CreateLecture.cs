using EventList.WebApi.Common;
using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Common.Interfaces;
using EventList.WebApi.Entities;
using EventList.WebApi.Infrastructure.Persistence;
using EventList.WebApi.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Lectures
{
    #region Api Endpoints
    public sealed class CreateLectureController : ApiControllerBase
    {
        [HttpPost("/api/lectures")]
        [SwaggerOperation(Tags = new[] { "Lectures" })]
        public async Task<ActionResult<int>> Create(CreateLectureCommand command)
        {
            return await Mediator.Send(command);
        }
    }
    #endregion

    #region Command definition with validation
    public sealed class CreateLectureCommand : IRequest<int>
    {
        public int EventId { get; set; }

        public IList<long> LecturersIds { get; set; }

        public Location Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? Name { get; set; }

        public string? Topic { get; set; }

        public string? Description { get; set; }

    }

    public sealed class CreateLectureCommandValidator : AbstractValidator<CreateLectureCommand>
    {
        public CreateLectureCommandValidator(IDateTime dateTimeProvider)
        {
            RuleFor(x => x.Name).NotEmpty()
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Topic).NotEmpty()
                .NotEmpty().WithMessage("Topic is required.")
                .MaximumLength(60).WithMessage("Topic must not exceed 60 characters.");

            RuleFor(x => x.Description).NotEmpty()
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(x => x.EventId).NotNull()
                .WithMessage("EventId is required");

            RuleFor(x => x.LecturersIds).NotEmpty()
                .WithMessage("LecturersIds is required");

            RuleFor(x => x.Location).NotNull()
                .WithMessage("Location is required");

            RuleFor(x => x.StartTime).GreaterThan(dateTimeProvider.Now)
                .WithMessage("Cannot schedule StartTime in the past");
        }
    }

    #endregion

    #region Command handler
    internal sealed class CreateLectureCommandHandler : IRequestHandler<CreateLectureCommand, int>
    {
        private readonly ApplicationDbContext _context;
        public CreateLectureCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateLectureCommand request, CancellationToken cancellationToken)
        {

            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

            if (@event is null)
                throw new NotFoundException("Event", request.EventId);

            var lecturers = await _context.Lecturers
                .Where(l => request.LecturersIds.Contains(l.Id))
                .ToListAsync(cancellationToken);

            var allLecturersFound = lecturers.Count() == request.LecturersIds.Distinct().Count();

            if (!allLecturersFound)
                throw new NotFoundException("Lecturer", request.LecturersIds);

            var lecture = new Lecture(
                @event.Id,
                lecturers,
                request.Location,
                request.StartTime,
                request.EndTime,
                request.Name,
                request.Topic,
                request.Description, @event
             );

            await _context.Lectures.AddAsync(lecture, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return lecture.Id;

        }
    }

    #endregion

}
