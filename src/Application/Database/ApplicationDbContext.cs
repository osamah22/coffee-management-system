using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace Application.Models;

public sealed class ApplicationDbContext : IdentityDbContext<User>
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