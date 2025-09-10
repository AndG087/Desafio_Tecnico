// Agenda.Application/Contacts/Handlers/SearchContactsHandler.cs
using Agenda.Infrastructure.Queries;
using MediatR;

public class SearchContactsHandler(ContactReadRepository readRepo)
    : IRequestHandler<SearchContactsQuery, (IEnumerable<ContactListItem>, int)>
{
    public Task<(IEnumerable<ContactListItem>, int)> Handle(SearchContactsQuery r, CancellationToken ct)
        => readRepo.SearchAsync(r.Term, r.Page, r.Size);
}
