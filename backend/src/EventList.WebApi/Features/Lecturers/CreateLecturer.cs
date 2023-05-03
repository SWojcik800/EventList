using EventList.WebApi.Common;
using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Entities;
using EventList.WebApi.Infrastructure.Persistence;
using EventList.WebApi.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Lecturers
{
    #region Api Endpoints
    public sealed class CreateLecturerController : ApiControllerBase
    {
        [HttpPost("/api/lecturers")]
        [SwaggerOperation(Tags = new[] { "Lecturers" })]
        public async Task<ActionResult<int>> Create(CreateLecturerCommand command)
        {
            return await Mediator.Send(command);
        }
    }
    #endregion

    #region Command definition with validation
    public sealed class CreateLecturerCommand : IRequest<int>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    public sealed class CreateLecturerCommandValidator : AbstractValidator<CreateLecturerCommand>
    {
        public CreateLecturerCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Description).NotEmpty()
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");
        }
    }
    #endregion

    #region Command handler
    internal sealed class CreateLecturerCommandHandler : IRequestHandler<CreateLecturerCommand, int>
    {
        private readonly ApplicationDbContext _context;
        public CreateLecturerCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateLecturerCommand request, CancellationToken cancellationToken)
        {
            var lecturer = new Lecturer(request.Name, request.Description);

            await _context.Lecturers.AddAsync(lecturer);
            await _context.SaveChangesAsync(cancellationToken);

            return lecturer.Id;

        }
    }

    #endregion
}
