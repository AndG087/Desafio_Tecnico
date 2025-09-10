// Agenda.Domain/Entities/Contact.cs
namespace Agenda.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; } = true;

    public void Update(string name, string email, string phone)
    {
        Name = name; Email = email; Phone = phone; UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;

    // Factory (regra básica de negócio)
    public static Contact Create(string name, string email, string phone)
    {
        // (regras simples) e.g.: nome >= 3, telefone só dígitos, email válido (validação robusta está no FluentValidation)
        return new Contact { Name = name, Email = email, Phone = phone };
    }
}
