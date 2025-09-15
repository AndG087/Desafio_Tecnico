using Agenda.Api.Data;
using Agenda.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class ContactsController(AgendaDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Paged<Contact>>> GetAll(
        [FromQuery] string? term = "",
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        var query = db.Contacts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(term))
            query = query.Where(c => c.Name.Contains(term) || c.Email.Contains(term));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return Ok(new Paged<Contact>(items, total));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> Get(Guid id)
    {
        var contact = await db.Contacts.FindAsync(id);
        return contact is null ? NotFound() : Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> Post(Contact contact)
    {
        db.Contacts.Add(contact);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Contact contact)
    {
        if (id != contact.Id) return BadRequest();
        db.Entry(contact).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var contact = await db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();
        db.Contacts.Remove(contact);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
