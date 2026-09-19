using System.Linq.Expressions;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public sealed class CoffeeRepository : ICoffeeRepository
{
    private readonly ApplicationDbContext _context;
    public CoffeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Coffee coffee, CancellationToken ct)
    {
        await _context.Coffees.AddAsync(coffee, ct);
    }
    public Task UpdateAsync(Coffee coffee, CancellationToken ct)
    {
        _context.Coffees.Update(coffee);
        return Task.CompletedTask;
    }
    public Task RemoveAsync(Coffee coffee, CancellationToken ct)
    {
        _context.Coffees.Remove(coffee);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Coffees
            .AnyAsync(c => c.Id == id, ct);
    }

    public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct)
    {
        return await _context.Coffees
            .AnyAsync(c => c.Slug == slug, ct);
    }
    public Task<Coffee?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _context.Coffees
            .Include(c => c.Options.OrderBy(o => o.Type).ThenBy(o => o.Size))
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }
    public Task<Coffee?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        return _context.Coffees
            .Include(c => c.Options.OrderBy(o => o.Type).ThenBy(o => o.Size))
            .FirstOrDefaultAsync(c => c.Slug == slug, ct);
    }
    public async Task<IList<Coffee>> GetAllAsync(Expression<Func<Coffee, bool>> expression,
     CancellationToken ct)
    {
        return await _context.Coffees
            .Include(c => c.Options.OrderBy(o => o.Type).ThenBy(o => o.Size))
            .Where(expression)
            .ToListAsync(ct);
    }
}
