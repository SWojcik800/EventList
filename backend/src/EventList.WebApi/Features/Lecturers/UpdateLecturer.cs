using EventList.WebApi.Common;
using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Common.Interfaces;
using EventList.WebApi.Features.Lectures;
using EventList.WebApi.Infrastructure.Persistence;
using EventList.WebApi.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Lecturers
{
    #region Api Endpoints
    public sealed class UpdateLecturerController : ApiControllerBase
    {
        [HttpPatch("/api/lecturers")]
        [SwaggerOperation(Tags = new[] { "Lecturers" })]
        public async Task Cancel(UpdateLecturerCommand command)
        {
            await Mediator.Send(command);
        }
    }
    #endregion

    #region Command definition with validation
    public sealed class UpdateLecturerCommand : IRequest
    {
        public int LecturerId { get; set; }

        public IList<int>? LectureIds { get; set; }

        public string? Description { get; set; }

    }

    public sealed class UpdateLecturerCommandValidator : AbstractValidator<UpdateLecturerCommand>
    {
        public UpdateLecturerCommandValidator(IDateTime dateTimeProvider)
        {
            RuleFor(x => x.LecturerId).NotEmpty()
                .WithMessage("LectureId is required");

        }
    }

    #endregion

    #region Command handler
    internal sealed class UpdateLecturerCommandHandler : IRequestHandler<UpdateLecturerCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTime _dateTimeProvider;

        public UpdateLecturerCommandHandler(ApplicationDbContext context, IDateTime dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(UpdateLecturerCommand request, CancellationToken cancellationToken)
        {
            var lecturer = await _context.Lecturers.Include(l => l.Lectures).FirstOrDefaultAsync(l => l.Id == request.LecturerId);

            if (lecturer is null)
                throw new NotFoundException("Lecturer", request.LecturerId);

            if (request.LectureIds is not null)
            {
                var lectures = await _context.Lectures.Where(l => request.LectureIds.Contains(l.Id))
                    .ToListAsync();

                var allLecturersFound = lectures.Count() == request.LectureIds.Distinct().Count();

                if (!allLecturersFound)
                    throw new NotFoundException("Lecture", request.LectureIds);

                lecturer.UpdateLectures(lectures);
            }

            if (!string.IsNullOrEmpty(request.Description))
                lecturer.Description = request.Description;

            _context.Update(lecturer);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion
}
