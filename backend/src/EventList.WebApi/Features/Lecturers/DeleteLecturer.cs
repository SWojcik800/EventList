using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Common;
using EventList.WebApi.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using EventList.WebApi.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventList.WebApi.Features.Lecturers
{
    #region Api Endpoints
    public sealed class DeleteLecturerController : ApiControllerBase
    {
        [HttpDelete("/api/lecturers")]
        [SwaggerOperation(Tags = new[] { "Lecturers" })]
        public async Task Cancel(DeleteLecturerCommand command)
        {
            await Mediator.Send(command);
        }
    }
    #endregion
    #region Command definition with validation
    public sealed class DeleteLecturerCommand : IRequest
    {
        public int LecturerId { get; set; }

    }

    public sealed class DeleteLecturerCommandValidator : AbstractValidator<DeleteLecturerCommand>
    {
        public DeleteLecturerCommandValidator(IDateTime dateTimeProvider)
        {
            RuleFor(x => x.LecturerId).NotEmpty()
                .WithMessage("LecturerId is required");
        }
    }

    #endregion

    #region Command handler
    internal sealed class DeleteLecturerCommandHandler : IRequestHandler<DeleteLecturerCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTime _dateTimeProvider;

        public DeleteLecturerCommandHandler(ApplicationDbContext context, IDateTime dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(DeleteLecturerCommand request, CancellationToken cancellationToken)
        {
            var lecturer = await _context.Lecturers
                .Include(x => x.Lectures)
                .ThenInclude(l => l.Lecturers)
                .FirstOrDefaultAsync(l => l.Id == request.LecturerId);

            if (lecturer is null)
                throw new NotFoundException("Lecturer", request.LecturerId);

            foreach (var lecture in lecturer.Lectures)
            {
                if (lecture.Lecturers.Count <= 1)
                    throw new ApplicationErrorException($"Cannot delete because no lecturer would be assigned to lecture {lecture.Name}");
            }

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }

    }

    #endregion
}
