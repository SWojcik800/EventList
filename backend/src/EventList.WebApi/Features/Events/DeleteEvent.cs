using EventList.WebApi.Common;
using EventList.WebApi.Common.Exceptions;
using EventList.WebApi.Entities;
using EventList.WebApi.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventList.WebApi.Features.Events;

public class DeleteEventController : ApiControllerBase
{
    [HttpDelete("/api/events/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteEventCommand { Id = id });

        return NoContent();
    }
}

public class DeleteEventCommand : IRequest
{
    public int Id { get; set; }
}

internal sealed class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteEventCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Events
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Event), request.Id);
        }

        _context.Events.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
