using Microsoft.EntityFrameworkCore;

namespace Application.Models;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Coffee> Coffees => Set<Coffee>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
     : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}