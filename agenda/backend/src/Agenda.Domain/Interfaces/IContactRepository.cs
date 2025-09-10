using Agenda.Domain.Entities;

namespace Agenda.Domain.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, Guid? exceptId, CancellationToken ct);
    Task AddAsync(Contact contact, CancellationToken ct);
    Task UpdateAsync(Contact contact, CancellationToken ct);
    Task DeleteAsync(Contact contact, CancellationToken ct);
}
