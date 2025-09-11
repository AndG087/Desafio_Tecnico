namespace Agenda.Api.Models;

public record Paged<T>(IEnumerable<T> Items, int Total);
