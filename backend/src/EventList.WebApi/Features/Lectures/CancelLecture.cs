using EventList.WebApi.Common;
using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Common.Interfaces;
using EventList.WebApi.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Lectures
{
    #region Api Endpoints
    public sealed class CancelLectureController : ApiControllerBase
    {
        [HttpDelete("/api/lectures")]
        [SwaggerOperation(Tags = new[] { "Lectures" })]
        public async Task Cancel(CancelLectureCommand command)
        {
            await Mediator.Send(command);
        }
    }
    #endregion
    #region Command definition with validation
    public sealed class CancelLectureCommand : IRequest
    {
        public int LectureId { get; set; }

    }

    public sealed class CancelLectureCommandValidator : AbstractValidator<CancelLectureCommand>
    {
        public CancelLectureCommandValidator(IDateTime dateTimeProvider)
        {
            RuleFor(x => x.LectureId).NotEmpty()
                .WithMessage("LectureId is required");
        }
    }

    #endregion

    #region Command handler
    internal sealed class CancelLectureCommandHandler : IRequestHandler<CancelLectureCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTime _dateTimeProvider;

        public CancelLectureCommandHandler(ApplicationDbContext context, IDateTime dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(CancelLectureCommand request, CancellationToken cancellationToken)
        {
            var lecture = await _context.Lectures.FirstOrDefaultAsync(l => l.Id == request.LectureId, cancellationToken);

            if(lecture is null)
                throw new NotFoundException("Lecture", request.LectureId);

            if (lecture.IsFinished(_dateTimeProvider))
                throw new ApplicationException("Cannot cancel lecture that has already finished");

            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync(cancellationToken);    
            
            return Unit.Value;
        }

    }

    #endregion
}
