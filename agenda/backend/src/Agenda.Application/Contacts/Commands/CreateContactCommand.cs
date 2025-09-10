// Agenda.Application/Contacts/Commands/CreateContactCommand.cs
using MediatR;

public record CreateContactCommand(string Name, string Email, string Phone) : IRequest<Guid>;
