using Microsoft.EntityFrameworkCore;
using portfolium.Core.Entities;

namespace portfolium.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) {
    public DbSet<Stock?> Stock { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Stock>()
                    .HasIndex(s => s.Symbol)
                    .IsUnique();
    }
}