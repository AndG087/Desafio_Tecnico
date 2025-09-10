// Agenda.Infrastructure/Persistence/ContactRepository.cs
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Persistence;

public class ContactRepository : IContactRepository
{
    private readonly AgendaDbContext _db;
    public ContactRepository(AgendaDbContext db) => _db = db;

    public Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Contacts.FirstOrDefaultAsync(x => x.Id == id && x.IsActive, ct);

    public Task<bool> EmailExistsAsync(string email, Guid? exceptId, CancellationToken ct)
        => _db.Contacts.AnyAsync(x => x.Email == email && x.IsActive && (exceptId == null || x.Id != exceptId), ct);

    public async Task AddAsync(Contact c, CancellationToken ct)
    {
        await _db.Contacts.AddAsync(c, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Contact c, CancellationToken ct)
    {
        _db.Contacts.Update(c);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Contact c, CancellationToken ct)
    {
        c.Deactivate();
        await _db.SaveChangesAsync(ct);
    }
}
