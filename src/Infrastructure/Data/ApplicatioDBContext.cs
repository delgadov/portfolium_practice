using Microsoft.EntityFrameworkCore;
using portfolium.Core.Entities;

namespace portfolium.Infrastructure.Data;

public class ApplicatioDBContext(DbContextOptions<ApplicatioDBContext> options) : DbContext(options) {
    public DbSet<Stock> Stock { get; set; }
}