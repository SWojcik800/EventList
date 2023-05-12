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
    public sealed class UpdateLectureInfoController : ApiControllerBase
    {
        [HttpPatch("/api/lectures")]
        [SwaggerOperation(Tags = new[] { "Lectures" })]
        public async Task Cancel(UpdateLectureInfoCommand command)
        {
            await Mediator.Send(command);
        }
    }
    #endregion
    #region Command definition with validation
    public sealed class UpdateLectureInfoCommand : IRequest
    {
        public int LectureId { get; set; }
        public IList<int>? LecturersIds { get; set; } = new List<int>();

        public Location? Location { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Name { get; set; }

        public string? Topic { get; set; }

        public string? Description { get; set; }

    }

    public sealed class UpdateLectureInfoCommandValidator : AbstractValidator<UpdateLectureInfoCommand>
    {
        public UpdateLectureInfoCommandValidator(IDateTime dateTimeProvider)
        {
            RuleFor(x => x.LectureId).NotEmpty()
                .WithMessage("LectureId is required");

        }
    }

    #endregion

    #region Command handler
    internal sealed class UpdateLectureInfoCommandHandler : IRequestHandler<UpdateLectureInfoCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTime _dateTimeProvider;

        public UpdateLectureInfoCommandHandler(ApplicationDbContext context, IDateTime dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(UpdateLectureInfoCommand request, CancellationToken cancellationToken)
        {
            var lecture = await _context.Lectures.Include(l => l.Lecturers).FirstOrDefaultAsync(l => l.Id == request.LectureId, cancellationToken: cancellationToken);

            if (lecture is null)
                throw new NotFoundException("Lecture", request.LectureId);

            if (lecture.IsFinished(_dateTimeProvider))
                throw new ApplicationException("Cannot update lecture that has already finished");

            if (request.LecturersIds is not null)
            {
                var lecturers = await _context.Lecturers.Where(l => request.LecturersIds.Contains(l.Id))
                    .ToListAsync(cancellationToken);

                var allLecturersFound = lecturers.Count() == request.LecturersIds.Distinct().Count();

                if (!allLecturersFound)
                    throw new NotFoundException("Lecturer", request.LecturersIds);

                lecture.ChangeLecturers(lecturers);

            }

            if (request.Location is not null)            
                lecture.Location = request.Location;

            if(request.StartTime is not null)
                lecture.StartTime = (DateTime)request.StartTime;

            if (request.EndTime is not null)
                lecture.EndTime = (DateTime)request.EndTime;

            if (!string.IsNullOrEmpty(request.Name))
                lecture.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Topic))
                lecture.Topic = request.Topic;

            if (!string.IsNullOrEmpty(request.Description))
                lecture.Description = request.Description;

            _context.Update(lecture);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    #endregion
}
