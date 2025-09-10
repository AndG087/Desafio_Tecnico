// Agenda.Api/Controllers/ContactsController.cs
using Agenda.Application.Contacts.Handlers;
using Agenda.Application.Contacts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/contacts")]
[Authorize] // proteger os endpoints
public class ContactsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? term, [FromQuery] int page = 1, [FromQuery] int size = 10)
        => Ok(await mediator.Send(new SearchContactsQuery(term, page, size)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await mediator.Send(new GetContactByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactCommand cmd)
    {
        var id = await mediator.Send(cmd);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommand cmd)
    {
        await mediator.Send(cmd with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteContactCommand(id));
        return NoContent();
    }
}
