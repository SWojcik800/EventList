using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Common;
using EventList.WebApi.Infrastructure.Persistence;
using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using EventList.WebApi.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace EventList.WebApi.Features.Events;

public class UpdateEventController : ApiControllerBase
{
    [HttpPut("/api/events/{id}")]
    [SwaggerOperation(Tags = new[] { "Events" })]
    public async Task<ActionResult> Update(int id, UpdateEventCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}

public class UpdateEventCommand : IRequest
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime StartDate { get; set; }
}

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateEventCommandValidator(ApplicationDbContext context)
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

    public Task<bool> BeUniqueName(UpdateEventCommand model, string name, CancellationToken cancellationToken)
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

internal sealed class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateEventCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Events
            .FindAsync(new object[] { request.Id }, cancellationToken)
            .ConfigureAwait(false);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Event), request.Id);
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
