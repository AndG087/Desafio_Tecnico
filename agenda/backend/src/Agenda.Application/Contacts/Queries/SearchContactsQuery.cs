// Agenda.Application/Contacts/Queries/SearchContactsQuery.cs
using MediatR;

public record SearchContactsQuery(string? Term, int Page = 1, int Size = 10) 
    : IRequest<(IEnumerable<ContactListItem> Items, int Total)>;
