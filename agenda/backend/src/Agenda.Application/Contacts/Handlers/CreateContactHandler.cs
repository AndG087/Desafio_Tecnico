// Agenda.Application/Contacts/Handlers/CreateContactHandler.cs
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using Agenda.Infrastructure.Messaging;
using MediatR;

public class CreateContactHandler(IContactRepository repo, RabbitPublisher publisher) 
    : IRequestHandler<CreateContactCommand, Guid>
{
    public async Task<Guid> Handle(CreateContactCommand r, CancellationToken ct)
    {
        if (await repo.EmailExistsAsync(r.Email, null, ct))
            throw new InvalidOperationException("E-mail já cadastrado.");

        var c = Contact.Create(r.Name, r.Email, r.Phone);
        await repo.AddAsync(c, ct);

        publisher.Publish("contact.created", new { c.Id, c.Name, c.Email, c.Phone, c.CreatedAt });
        return c.Id;
    }
}
