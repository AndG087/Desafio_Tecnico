using Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Persistence;

public class AgendaDbContext : DbContext
{
    public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options) {}
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Contact>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.Email).HasMaxLength(180).IsRequired();
            e.Property(x => x.Phone).HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });
    }
}
