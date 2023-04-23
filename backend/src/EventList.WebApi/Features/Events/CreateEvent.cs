using EventList.WebApi.Common;
using EventList.WebApi.Entities;
using EventList.WebApi.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Events
{
    public class CreateEventController : ApiControllerBase
    {
        [HttpPost("/api/events")]
        [SwaggerOperation(Tags = new[] { "Events" })]
        public async Task<ActionResult<int>> Create(CreateEventCommand command)
        {
            return await Mediator.Send(command);
        }
    }

    public class CreateEventCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateEventCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");

            RuleFor(v => v.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .MustAsync(StartInTheFutureOrToday).WithMessage("The specified start date is in the past.");
        }

        private Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return _context.Events
                .AllAsync(l => l.Name != name, cancellationToken);
        }

        private Task<bool> StartInTheFutureOrToday(DateTime startDate, CancellationToken cancellationToken)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var spanToStartDate = startDate - yesterday;
            var isDateValid = spanToStartDate.Days > 0;

            return Task.FromResult(isDateValid);
        }
    }

    internal sealed class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateEventCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var entity = new Event { Name = request.Name, StartDate = request.StartDate };

            _context.Events.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }


}
