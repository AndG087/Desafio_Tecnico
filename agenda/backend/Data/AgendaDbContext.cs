using Agenda.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Api.Data;

public class AgendaDbContext(DbContextOptions<AgendaDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var c = modelBuilder.Entity<Contact>();
        c.HasKey(x => x.Id);
        c.Property(x => x.Name).IsRequired().HasMaxLength(120);
        c.Property(x => x.Email).IsRequired().HasMaxLength(180);
        c.Property(x => x.Phone).IsRequired().HasMaxLength(40);
    }
}
