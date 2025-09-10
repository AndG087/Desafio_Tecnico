// Agenda.Infrastructure/Queries/ContactReadRepository.cs  (Dapper para listagem/pesquisa)
using Dapper;
using Npgsql;

namespace Agenda.Infrastructure.Queries;

public record ContactListItem(Guid Id, string Name, string Email, string Phone);

public class ContactReadRepository
{
    private readonly string _conn;
    public ContactReadRepository(IConfiguration cfg) => _conn = cfg.GetConnectionString("DefaultConnection")!;
    public async Task<(IEnumerable<ContactListItem> Items, int Total)> SearchAsync(string? term, int page, int size)
    {
        await using var cn = new NpgsqlConnection(_conn);
        var where = "WHERE \"IsActive\" = true";
        if (!string.IsNullOrWhiteSpace(term))
            where += " AND (\"Name\" ILIKE @t OR \"Email\" ILIKE @t OR \"Phone\" ILIKE @t)";
        var sql = $@"
            SELECT ""Id"", ""Name"", ""Email"", ""Phone"" FROM ""Contacts""
            {where}
            ORDER BY ""CreatedAt"" DESC
            OFFSET @o LIMIT @l;
            SELECT COUNT(*) FROM ""Contacts"" {where};";
        using var multi = await cn.QueryMultipleAsync(sql, new { t = $"%{term}%", o = (page-1)*size, l = size });
        var items = await multi.ReadAsync<ContactListItem>();
        var total = await multi.ReadFirstAsync<int>();
        return (items, total);
    }
}
